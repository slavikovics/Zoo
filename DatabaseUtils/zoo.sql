DROP TABLE IF EXISTS Animals CASCADE;
DROP TABLE IF EXISTS BirdsWinterPlaces CASCADE;
DROP TABLE IF EXISTS ReptilesInfo CASCADE;
DROP TABLE IF EXISTS Employees CASCADE;
DROP TABLE IF EXISTS Diets CASCADE;
DROP TABLE IF EXISTS DietTypes CASCADE;
DROP TABLE IF EXISTS HabitatZones CASCADE;
DROP TABLE IF EXISTS AnimalTypes CASCADE;
DROP TABLE IF EXISTS AnimalVet CASCADE;

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
    FOREIGN KEY (MarriedWith) REFERENCES Employees(Id) ON DELETE SET NULL
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
    FOREIGN KEY (TypeId) REFERENCES DietTypes(Id) ON DELETE RESTRICT
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
     DietId INT NULL,
     HabitatZoneId INT NOT NULL,
     Vet INT NOT NULL,
     CaretakerId INT NOT NULL,
     FOREIGN KEY (TypeId) REFERENCES AnimalTypes(Id) ON DELETE RESTRICT,
     FOREIGN KEY (WinterPlaceId) REFERENCES BirdsWinterPlaces(Id) ON DELETE SET NULL,
     FOREIGN KEY (ReptileInfoId) REFERENCES ReptilesInfo(Id) ON DELETE SET NULL,
     FOREIGN KEY (DietId) REFERENCES Diets(Id) ON DELETE SET NULL,
     FOREIGN KEY (HabitatZoneId) REFERENCES HabitatZones(Id) ON DELETE RESTRICT,
     FOREIGN KEY (CaretakerId) REFERENCES Employees(Id) ON DELETE RESTRICT
);

CREATE TABLE AnimalVet (
    AnimalId INT,
    VetId INT,
    PRIMARY KEY (AnimalId, VetId),
    FOREIGN KEY (AnimalId) REFERENCES Animals(Id) ON DELETE CASCADE,
    FOREIGN KEY (VetId) REFERENCES Employees(Id) ON DELETE CASCADE
);

-- 1. Типы животных
INSERT INTO AnimalTypes (Id, Name) VALUES
    (1, 'Млекопитающее'),
    (2, 'Птица'),
    (3, 'Рептилия'),
    (4, 'Амфибия'),
    (5, 'Рыба'),
    (6, 'Насекомое'),
    (7, 'Паукообразное'),
    (8, 'Грызун'),
    (9, 'Хищник'),
    (10, 'Примат');

-- 2. Сотрудники
INSERT INTO Employees (Id, Name, BirthDate, PhoneNumber, MaritalStatus, MarriedWith) VALUES
    (1, 'Иван Петров', '1985-03-15', '+7-912-345-67-89', 'Married', 2),
    (2, 'Мария Сидорова', '1988-07-22', '+7-912-345-67-90', 'Married', 1),
    (3, 'Алексей Козлов', '1990-11-05', '+7-912-345-67-91', 'Single', NULL),
    (4, 'Елена Новикова', '1992-01-30', '+7-912-345-67-92', 'Divorced', NULL),
    (5, 'Дмитрий Волков', '1987-09-14', '+7-912-345-67-93', 'Married', 6),
    (6, 'Ольга Орлова', '1989-04-18', '+7-912-345-67-94', 'Married', 5),
    (7, 'Сергей Павлов', '1993-12-03', '+7-912-345-67-95', 'Single', NULL),
    (8, 'Анна Кузнецова', '1991-06-25', '+7-912-345-67-96', 'Widowed', NULL),
    (9, 'Михаил Лебедев', '1986-08-11', '+7-912-345-67-97', 'Single', NULL),
    (10, 'Наталья Соколова', '1994-02-14', '+7-912-345-67-98', 'Single', NULL),
    (11, 'Виктор Морозов', '1984-05-09', '+7-912-345-67-99', 'Married', 12),
    (12, 'Ирина Зайцева', '1987-10-28', '+7-912-345-68-00', 'Married', 11),
    (13, 'Павел Громов', '1995-07-17', '+7-912-345-68-01', 'Single', NULL),
    (14, 'Светлана Воробьева', '1990-03-22', '+7-912-345-68-02', 'Single', NULL),
    (15, 'Андрей Титов', '1983-11-08', '+7-912-345-68-03', 'Divorced', NULL);

-- 3. Места зимовки птиц
INSERT INTO BirdsWinterPlaces (Id, CountryName, Departure, Arrival) VALUES
    (1, 'Египет', '2024-09-15', '2025-03-20'),
    (2, 'Индия', '2024-10-01', '2025-04-10'),
    (3, 'Южная Африка', '2024-08-20', '2025-02-28'),
    (4, 'Австралия', '2024-09-25', '2025-03-15'),
    (5, 'Таиланд', '2024-10-05', '2025-04-05'),
    (6, 'Испания', '2024-09-10', '2025-03-25'),
    (7, 'Греция', '2024-09-20', '2025-03-30'),
    (8, 'Мексика', '2024-08-30', '2025-02-20'),
    (9, 'Бразилия', '2024-09-05', '2025-03-10'),
    (10, 'Вьетнам', '2024-10-10', '2025-04-15');

-- 4. Типы рационов
INSERT INTO DietTypes (Id, Type) VALUES
    (1, 'Детский'),
    (2, 'Диетический'),
    (3, 'Усиленный'),
    (4, 'Стандартный'),
    (5, 'Специализированный'),
    (6, 'Лечебный'),
    (7, 'Экзотический'),
    (8, 'Сезонный'),
    (9, 'Энергетический'),
    (10, 'Щадящий');

-- 5. Рационы
INSERT INTO Diets (Id, Name, TypeId, Description) VALUES
    (1, 'Молочная смесь для детенышей', 1, 'Специальная молочная смесь для вскармливания новорожденных животных'),
    (2, 'Легкий корм для птиц', 2, 'Сбалансированный корм с пониженным содержанием жиров'),
    (3, 'Энергетический рацион для хищников', 3, 'Высокобелковый рацион с добавлением витаминов'),
    (4, 'Стандартный растительный', 4, 'Основной рацион для травоядных животных'),
    (5, 'Специальный для рептилий', 5, 'Рацион с добавлением кальция и ультрафиолетовой обработкой'),
    (6, 'Восстановительный', 6, 'Лечебное питание для животных после болезни'),
    (7, 'Тропические фрукты', 7, 'Смесь экзотических фруктов для тропических видов'),
    (8, 'Летний рацион', 8, 'Сезонное питание с увеличенным содержанием жидкости'),
    (9, 'Зимний усиленный', 9, 'Высококалорийный рацион для зимнего периода'),
    (10, 'Щадящий для ЖКТ', 10, 'Легкоусвояемое питание для животных с чувствительным пищеварением'),
    (11, 'Рыбный белковый', 3, 'Рацион на основе рыбы для водных хищников'),
    (12, 'Овощной микс', 4, 'Смесь свежих овощей для грызунов'),
    (13, 'Насекомые и личинки', 7, 'Живые корма для насекомоядных видов'),
    (14, 'Мясной ассорти', 3, 'Разнообразные мясные продукты для крупных хищников'),
    (15, 'Зерновая смесь', 4, 'Сбалансированная зерновая смесь для птиц');

-- 6. Зоны обитания
INSERT INTO HabitatZones (Id, Name, Description) VALUES
    (1, 'Африканская саванна', 'Открытые пространства с травянистой растительностью, имитирующие африканские равнины'),
    (2, 'Тропический лес', 'Влажный климат с обильной растительностью, подходит для тропических видов'),
    (3, 'Полярная зона', 'Холодный климат с искусственным снегом и льдом для арктических животных'),
    (4, 'Пустыня', 'Сухой климат с песчаными дюнами и кактусами'),
    (5, 'Акватория', 'Большие бассейны и аквариумы для водных обитателей'),
    (6, 'Ночной мир', 'Освещение, имитирующее ночное время для ночных животных'),
    (7, 'Детский зоопарк', 'Контактная зона с домашними животными'),
    (8, 'Обезьянник', 'Вольеры с климат-установками для приматов'),
    (9, 'Птичий вольер', 'Большие сетчатые вольеры для свободного полета птиц'),
    (10, 'Террариум', 'Специальные помещения для рептилий и амфибий'),
    (11, 'Скалистый ландшафт', 'Имитация горной местности с камнями и утесами'),
    (12, 'Азиатские джунгли', 'Густые заросли с водопадами для азиатских видов'),
    (13, 'Южноамериканская пампa', 'Открытые пространства с высокой травой'),
    (14, 'Австралийский буш', 'Зона с эвкалиптами и характерным ландшафтом'),
    (15, 'Европейский лес', 'Лиственные и хвойные деревья умеренного пояса');

-- 7. Информация о рептилиях
INSERT INTO ReptilesInfo (Id, NormalTemperature, SleepStart, SleepEnd) VALUES
    (1, 28.50, '2024-11-15', '2025-02-28'),
    (2, 30.00, '2024-10-20', '2025-03-15'),
    (3, 25.50, NULL, NULL),
    (4, 32.00, '2024-12-01', '2025-01-31'),
    (5, 27.50, '2024-11-10', '2025-03-10'),
    (6, 29.00, NULL, NULL),
    (7, 31.50, '2024-10-25', '2025-02-20'),
    (8, 26.00, '2024-11-05', '2025-03-05'),
    (9, 33.00, NULL, NULL),
    (10, 28.00, '2024-12-10', '2025-01-20'),
    (11, 30.50, '2024-11-20', '2025-02-15'),
    (12, 24.50, NULL, NULL),
    (13, 29.50, '2024-10-30', '2025-03-20'),
    (14, 31.00, NULL, NULL),
    (15, 27.00, '2024-11-25', '2025-02-10');

-- 8. Животные
INSERT INTO Animals (Id, Name, TypeId, BirthDate, Sex, WinterPlaceId, ReptileInfoId, DietId, HabitatZoneId, Vet, CaretakerId) VALUES
-- Млекопитающие
    (1, 'Симба', 1, '2020-05-15', 'Male', NULL, NULL, 3, 1, 1, 3),
    (2, 'Багира', 1, '2019-08-20', 'Female', NULL, NULL, 3, 2, 1, 3),
    (3, 'Балто', 9, '2018-03-10', 'Male', NULL, NULL, 14, 3, 2, 4),
    (4, 'Чита', 9, '2021-01-25', 'Female', NULL, NULL, 14, 1, 2, 4),
    (5, 'Фунтик', 10, '2022-06-12', 'Male', NULL, NULL, 7, 8, 5, 7),
    (6, 'Коко', 10, '2015-11-30', 'Female', NULL, NULL, 7, 8, 5, 7),
    (7, 'Бемби', 1, '2023-04-05', 'Male', NULL, NULL, 1, 15, 6, 8),
    (8, 'Рексик', 8, '2022-09-18', 'Male', NULL, NULL, 12, 7, 6, 8),

-- Птицы
    (9, 'Кеша', 2, '2021-07-22', 'Male', 1, NULL, 2, 9, 9, 10),
    (10, 'Лора', 2, '2020-12-14', 'Female', 2, NULL, 15, 9, 9, 10),
    (11, 'Гоша', 2, '2019-04-30', 'Male', 3, NULL, 2, 9, 11, 13),
    (12, 'Икар', 2, '2022-03-08', 'Male', 4, NULL, 15, 9, 11, 13),

-- Рептилии
    (13, 'Гена', 3, '2018-05-20', 'Male', NULL, 1, 5, 10, 12, 14),
    (14, 'Вася', 3, '2017-11-12', 'Male', NULL, 2, 5, 10, 12, 14),
    (15, 'Зоя', 3, '2020-02-28', 'Female', NULL, 3, 5, 10, 15, 1),
    (16, 'Тиша', 3, '2019-07-15', 'Female', NULL, 4, 5, 10, 15, 1),

-- Другие
    (17, 'Квакша', 4, '2021-09-09', 'Female', NULL, NULL, 13, 10, 2, 3),
    (18, 'Немо', 5, '2022-12-03', 'Male', NULL, NULL, 11, 5, 5, 7),
    (19, 'Баззи', 6, '2023-01-15', 'Male', NULL, NULL, 13, 6, 6, 8),
    (20, 'Шелдон', 7, '2020-10-20', 'Male', NULL, NULL, 13, 10, 9, 10);

-- 9. Связь животных с ветеринарами (AnimalVet)
INSERT INTO AnimalVet (AnimalId, VetId) VALUES
-- У некоторых животных несколько ветеринаров
    (1, 1), (1, 2),
    (2, 1), (2, 5),
    (3, 2), (3, 6),
    (4, 2),
    (5, 5), (5, 9),
    (6, 5),
    (7, 6), (7, 11),
    (8, 6),
    (9, 9), (9, 12),
    (10, 9),
    (11, 11), (11, 15),
    (12, 11),
    (13, 12), (13, 2),
    (14, 12),
    (15, 15), (15, 1),
    (16, 15),
    (17, 2),
    (18, 5),
    (19, 6),
    (20, 9);