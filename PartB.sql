CREATE DATABASE IF NOT EXISTS family_members;
USE family_members;

CREATE TABLE IF NOT EXISTS people (
    Person_Id INT PRIMARY KEY,       
    Personal_Name VARCHAR(100) NOT NULL, 
    Family_Name VARCHAR(100) NOT NULL, 
    Gender CHAR(1) NOT NULL,             
    Father_Id INT,                     
    Mother_Id INT,                    
    Spouse_Id INT,                     
    CONSTRAINT fk_father FOREIGN KEY (Father_Id) REFERENCES people(Person_Id) ON DELETE SET NULL,
    CONSTRAINT fk_mother FOREIGN KEY (Mother_Id) REFERENCES people(Person_Id) ON DELETE SET NULL,
    CONSTRAINT fk_spouse FOREIGN KEY (Spouse_Id) REFERENCES people(Person_Id) ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS family_connections (
    Person_Id INT,                
    Relative_Id INT,                   
    Connection_Type VARCHAR(50) NOT NULL, 
    PRIMARY KEY (Person_Id, Relative_Id),
    CONSTRAINT fk_person FOREIGN KEY (Person_Id) REFERENCES people(Person_Id),
    CONSTRAINT fk_relative FOREIGN KEY (Relative_Id) REFERENCES people(Person_Id)
);

-- חיבור אנשים לרשומות הקשרים על פי ההגדרות
SELECT 
    p.Person_Id,
    r.Person_Id AS Relative_Id,
    CASE
        WHEN p.Father_Id = r.Person_Id THEN 'Father'
        WHEN p.Mother_Id = r.Person_Id THEN 'Mother'
        WHEN (p.Person_Id = r.Father_Id OR p.Person_Id = r.Mother_Id) AND p.Gender = 'M' THEN 'Son'
        WHEN (p.Person_Id = r.Father_Id OR p.Person_Id = r.Mother_Id) AND p.Gender = 'F' THEN 'Daughter'
        WHEN p.Spouse_Id IS NOT NULL AND p.Spouse_Id = r.Person_Id THEN 
            CASE WHEN p.Gender = 'M' THEN 'Husband' ELSE 'Wife' END
        WHEN (p.Father_Id = r.Father_Id OR p.Mother_Id = r.Mother_Id) 
             AND p.Person_Id != r.Person_Id THEN 
            CASE WHEN p.Gender = 'M' THEN 'Brother' ELSE 'Sister' END
        ELSE 'Unknown'
    END AS Connection_Type
FROM 
    people p
JOIN 
    people r ON p.Person_Id != r.Person_Id
ORDER BY 
    p.Person_Id, r.Person_Id;

-- EX2: עדכון בני זוג חסרים
SET SQL_SAFE_UPDATES = 0;

UPDATE people p1
JOIN people p2 ON p1.Spouse_Id = p2.Person_Id
SET p2.Spouse_Id = p1.Person_Id
WHERE p1.Spouse_Id IS NOT NULL
  AND (p2.Spouse_Id IS NULL OR p2.Spouse_Id != p1.Person_Id);

SET SQL_SAFE_UPDATES = 1;

-- הצגת פרטי בני הזוג לאחר העדכון
SELECT Person_Id, Personal_Name, Spouse_Id
FROM people
ORDER BY Person_Id;
