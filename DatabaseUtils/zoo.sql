DROP TABLE IF EXISTS Animals;
DROP TABLE IF EXISTS BirdsWinterPlaces;
DROP TABLE IF EXISTS ReptilesInfo;
DROP TABLE IF EXISTS Employees;
DROP TABLE IF EXISTS Diets;
DROP TABLE IF EXISTS DietTypes;
DROP TABLE IF EXISTS HabitatZones;
DROP TABLE IF EXISTS AnimalTypes;

CREATE TABLE AnimalTypes (
    Id INT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Employees (
    Id INT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    BirthDate DATE NOT NULL,
    PhoneNumber VARCHAR(50) NOT NULL,
    MaritalStatus VARCHAR(50) NOT NULL,
    MarriedWith INT NULL,
    FOREIGN KEY (MarriedWith) REFERENCES Employees(Id)
);

CREATE TABLE BirdsWinterPlaces (
    Id INT PRIMARY KEY,
    CountryName VARCHAR(50) NOT NULL,
    Departure DATE NOT NULL,
    Arrival DATE NOT NULL
);

CREATE TABLE DietTypes (
    Id INT PRIMARY KEY,
    Type VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Diets (
    Id INT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    TypeId INT NOT NULL,
    Description VARCHAR(500),
    FOREIGN KEY (TypeId) REFERENCES DietTypes(Id)
);

CREATE TABLE HabitatZones (
    Id INT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(500)
);

CREATE TABLE ReptilesInfo (
    Id INT PRIMARY KEY,
    NormalTemperature DECIMAL(4,2) NOT NULL,
    SleepStart DATE NULL,
    SleepEnd DATE NULL
);

CREATE TABLE Animals (
    Id INT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    TypeId INT NOT NULL,
    BirthDate DATE NULL,
    Sex VARCHAR(50) NOT NULL,
    WinterPlaceId INT NULL,
    ReptileInfoId INT NULL,
    DietId INT NOT NULL,
    HabitatZoneId INT NOT NULL,
    Vet INT NOT NULL,
    CaretakerId INT NOT NULL,
    FOREIGN KEY (TypeId) REFERENCES AnimalTypes(Id),
    FOREIGN KEY (WinterPlaceId) REFERENCES BirdsWinterPlaces(Id),
    FOREIGN KEY (ReptileInfoId) REFERENCES ReptilesInfo(Id),
    FOREIGN KEY (DietId) REFERENCES Diets(Id),
    FOREIGN KEY (HabitatZoneId) REFERENCES HabitatZones(Id),
    FOREIGN KEY (Vet) REFERENCES Employees(Id),
    FOREIGN KEY (CaretakerId) REFERENCES Employees(Id)
);

INSERT INTO AnimalTypes (Id, Name)
VALUES
(1, 'Птица'),
(2, 'Рептилия'),
(3, 'Млекопитающее');

INSERT INTO Employees (Id, Name, BirthDate, PhoneNumber, MaritalStatus, MarriedWith)
VALUES
(1, 'Иван Петров', '1980-05-15', '+7-912-345-67-89', 'Женат', NULL),
(2, 'Мария Сидорова', '1985-03-22', '+7-923-456-78-90', 'Замужем', NULL),
(3, 'Алексей Козлов', '1990-11-10', '+7-934-567-89-01', 'Холост', NULL),
(4, 'Ольга Новикова', '1992-07-30', '+7-945-678-90-12', 'Замужем', NULL);

UPDATE Employees SET MarriedWith = 2 WHERE Id = 1;
UPDATE Employees SET MarriedWith = 1 WHERE Id = 2;
UPDATE Employees SET MarriedWith = 1 WHERE Id = 4;
UPDATE Employees SET MaritalStatus = 'Женат' WHERE Id = 3;
UPDATE Employees SET MarriedWith = 3 WHERE Id = 4;
UPDATE Employees SET MarriedWith = 4 WHERE Id = 3;

INSERT INTO BirdsWinterPlaces (Id, CountryName, Departure, Arrival)
VALUES
(1, 'Египет', '2023-09-15', '2024-03-20'),
(2, 'Индия', '2023-10-01', '2024-04-05');

INSERT INTO DietTypes (Id, Type)
VALUES
(1, 'Детский'),
(2, 'Диетический'),
(3, 'Усиленный'),
(4, 'Стандартный');

INSERT INTO Diets (Id, Name, TypeId, Description)
VALUES
(1, 'Каша с витаминами', 1, 'Овсяная каша с добавлением витаминного комплекса для молодняка'),
(2, 'Постное мясо', 2, 'Нежирное мясо с овощами для животных с проблемами пищеварения'),
(3, 'Двойная порция мяса', 3, 'Усиленная мясная диета для хищников'),
(4, 'Фрукты и орехи', 4, 'Стандартный рацион для приматов');

INSERT INTO HabitatZones (Id, Name, Description)
VALUES
(1, 'Тропический лес', 'Высокая влажность, густая растительность, искусственный водопад.'),
(2, 'Саванна', 'Открытое пространство с небольшими деревьями и искусственным водоемом.'),
(3, 'Пустыня', 'Песчаный вольер с участками камней и системой обогрева.');

INSERT INTO ReptilesInfo (Id, NormalTemperature, SleepStart, SleepEnd)
VALUES
(1, 28.50, '2023-10-31', '2024-03-15'),
(2, 30.00, NULL, NULL);

INSERT INTO Animals (Id, Name, TypeId, BirthDate, Sex, WinterPlaceId, ReptileInfoId, DietId, HabitatZoneId, Vet, CaretakerId)
VALUES
(1, 'Ара', 1, '2018-06-10', 'Самец', 1, NULL, 4, 1, 1, 2),
(2, 'Черепаха', 2, '2010-08-22', 'Самка', NULL, 1, 2, 3, 1, 3),
(3, 'Варан', 2, '2019-11-05', 'Самец', NULL, 2, 3, 3, 4, 3),
(4, 'Шимпанзе', 3, '2015-04-18', 'Самка', NULL, NULL, 4, 1, 2, 4);