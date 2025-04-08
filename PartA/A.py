import os
from collections import Counter
import re


def split_log_file(input_file, output_dir="logs_split", lines_per_file=100000):
    """
    Splits the log file into smaller files.
    """
    os.makedirs(output_dir, exist_ok=True)
    file_count = 0
    output_files = []
    
    try:
        with open(input_file, 'r', encoding='utf-8') as infile:
            while True:
                lines = [infile.readline() for _ in range(lines_per_file)]
                lines = [line for line in lines if line.strip()] 
                if not lines: 
                    break  

                file_count += 1
                output_file = os.path.join(output_dir, f'split_{file_count}.txt')
                output_files.append(output_file)

                with open(output_file, 'w', encoding='utf-8') as outfile:
                    outfile.writelines(lines)
    except FileNotFoundError:
        print(f"Error: The input file {input_file} was not found.")
        return []
    except IOError as e:
        print(f"Error reading or writing file: {e}")
        return []

    return output_files


def count_error_codes(file_path):
    """
    Counts the error codes in a single file.
    """
    error_counts = Counter()
    error_pattern = re.compile(r'Error: (\S+)')

    try:
        with open(file_path, 'r', encoding='utf-8') as infile:
            for line in infile:
                match = error_pattern.search(line)
                if match:
                    error_code = match.group(1)
                    error_counts[error_code] += 1
    except IOError as e:
        print(f"Error reading file {file_path}: {e}")

    return error_counts


def merge_top_error_codes(log_dir="logs_split", top_N=5):
    """
    Merges the error code counts from all split log files and keeps only the top N.
    """
    total_error_counts = Counter()

    try:
        for file_name in os.listdir(log_dir):
            file_path = os.path.join(log_dir, file_name)
            if os.path.isfile(file_path) and file_name.startswith("split_") and file_name.endswith(".txt"):
                file_counts = count_error_codes(file_path)
                total_error_counts.update(file_counts)
    except FileNotFoundError:
        print(f"Error: The directory {log_dir} was not found.")
        return []
    except IOError as e:
        print(f"Error reading directory {log_dir}: {e}")

    return total_error_counts.most_common(top_N)


def display_top_N_error_codes(top_error_codes):
    """
    Displays the top N most common error codes.
    """
    print("\nTop Error Codes:")
    for i, (code, count) in enumerate(top_error_codes, start=1):
        print(f"{i}. {code}: {count} occurrences")


def main():
    """
    Main function that orchestrates the entire process.
    """
    input_file = "logs.txt"  
    N_top = 5  

    print("\nSplitting log file into smaller chunks...")
    split_log_file(input_file, lines_per_file=100000)

    print("\nCounting top error codes from all parts...")
    top_error_codes = merge_top_error_codes(top_N=N_top)

    if top_error_codes:
        print(f"\nDisplaying the top {N_top} most frequent error codes:")
        display_top_N_error_codes(top_error_codes)


if __name__ == "__main__":
    main()

"""
    Time complexity:
    Counting errors in each file: O(N*L), where N is the number of lines and L is the average length of a line.
    Merging all errors: O(E), where E is the number of unique errors.
    Sorting and finding the N most common: O(E log E).
    Total: O(N*L + E log E)

    Space complexity:
    We only keep a Counter of the unique errors, which is O(E).
"""
