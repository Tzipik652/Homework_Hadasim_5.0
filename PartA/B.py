import os
import pandas as pd

def validate_and_process(file_path):
    """
        Reads the file, performs checks on the 'timestamp' and 'value' columns,
        fixes invalid duplicates.
    """
    if file_path.endswith(".csv"):
        df = pd.read_csv(file_path)
    elif file_path.endwith(".parquet"):
        df = pd.read_parquet(file_path)

    # Converts timestamp to datetime format
    df['timestamp'] = pd.to_datetime(df['timestamp'], format='%d/%m/%Y %H:%M', errors='coerce')

    print("\033[32m Dates processed successfully.\033[0m")

    # Converts value to numbers, invalids will be NaN
    df["value"] = pd.to_numeric(df["value"], errors="coerce")
    invalid_rows = df[df["value"].isna()]

    if not invalid_rows.empty:
        print("\n\033[31m Found invalid or missing values ​​in 'value' column:\033[0m")
        print(invalid_rows)
    else:
        print("\033[32m All values ​​in 'value' column are valid.\033[0m")

    # Delete rows with missing values
    df.dropna(subset=["value"], inplace=True)

    # Handle duplicates – Calculate the average of duplicate values
    df = df.groupby('timestamp', as_index=False).agg({'value': 'mean'})

    print("\033[32m The data has been cleaned.\033[0m")
    return df

def calculate_hourly_average(df):
    """
        Calculates an average for each hour.
    """
    df['hour'] = df['timestamp'].dt.floor('h')
    hourly_avg = df.groupby('hour')['value'].mean().reset_index()

    print("\n \033[32m Hourly averages calculated: \033[0m")
    print(hourly_avg)

    return hourly_avg

def split_by_day(df, output_folder):
    """
        Splits the data into daily files.
        We will split the file into Parquet files, these are the advantages of working with them:
        - Better compression – Parquet files are significantly smaller than CSV files.
        - Faster reading and writing – Parquet allows selective loading of data (Columnar Storage).
        - Support for complex data types – CSV stores everything as strings, while Parquet stores the original types.
        - Optimal for Big Data – Suitable for working with Pandas, Apache Spark, Dask and more.
        - Direct access to specific columns – When reading, you can choose which columns to load instead of the entire file.
    """
    os.makedirs(output_folder, exist_ok=True)
    for date, group in df.groupby(df['timestamp'].dt.date):
        output_file = os.path.join(output_folder, f"{date}.parquet")
        group.to_parquet(output_file, index=False)
        print(f"Saved {output_file}")

def calculate_hourly_avg_for_files(input_folder, output_file):
    """
        Calculate hourly averages for each file daily and combines them into a final result.
    """
    all_dfs = []

    for filename in os.listdir(input_folder):
        if filename.endswith(".parquet"):
            file_path = os.path.join(input_folder, filename)
            df = pd.read_parquet(file_path)
         


            # Convert timestamp to datetime format
            df['timestamp'] = pd.to_datetime(df['timestamp'], format='%Y-%m-%d %H:%M:%S')

            # Calculate hourly averages
            df['hour'] = df['timestamp'].dt.floor('h')
            hourly_avg = df.groupby('hour')['value'].mean().reset_index()

            all_dfs.append(hourly_avg)

    # Consolidate all results
    final_df = pd.concat(all_dfs).sort_values('hour')
    # Save the final result 
    if output_file.endswith('.csv'):
        final_df.to_csv(output_file, index=False)
    elif output_file.endswith('.parquet'): #  Big Data זה  פתרון לקריאה וכתיבה מהירה, במיוחד בקבצי to_parquet()
        final_df.to_parquet(output_file, index=False)
    else:
        raise ValueError("Unsupported file format. Use CSV or Parquet.")
    print(f" Saved final result to {output_file}")

def main(): 
    file_path = 'time_series.parquet' 
    file_path = 'time_series.csv' 
    processed_data = validate_and_process(file_path) 

    output_folder = 'daily_files' 
    split_by_day(processed_data, output_folder) 

    output_file = 'hourly_averages.csv' 
    calculate_hourly_avg_for_files(output_folder, output_file)
  
if __name__ == "__main__": 
    main()

    """
    פתרון לשאלה 3 – עדכון ממוצעים שעתיים בזמן אמת
    שימוש בחישוב מצטבר (Online Aggregation)
    ,במקום לאגור את כל הנתונים
    נשתמש במבנה נתונים (כגון מילון)
    :שיאחסן לכל שעה את
    סכום הערכים שהתקבלו עד כה.1 
    מספר המדידות באותה שעה.2
    חישוב ממוצע תוך עדכון דינמי.3
        new_avg=(sum+new_value)/(count+1)

    Solution to Question 3 – Updating hourly averages in real time
    Using Online Aggregation
    Instead of storing all the data,
    we will use a data structure (such as a dictionary)
    that will store for each hour:
    1. the sum of the values ​​received so far
    2. The number of measurements in that hour
    3. Calculating the average while dynamically updating
        new_avg = (sum+new_value) / (count+1)

    """