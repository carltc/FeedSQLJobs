--INSERT INTO LiveMeals (
--	[MealName],
--	[ChefID],
--	[ChefName],
--	[MealPrice],
--	[MealLocation],
--	[MealTime],
--    [dietary_Meat],
--    [dietary_Shellfish],
--    [dietary_Fish],
--    [dietary_Gluten],
--    [dietary_Nut],
--    [dietary_Dairy],
--    [dietary_Egg],
--    [MealSpaces],
--    [Description],
--    [MealType],
--    [MealStatus]
--)
--VALUES (

--);

--DECLARE @tempTable TABLE (NAME Varchar(20))

DECLARE @smalldatetime smalldatetime = getdate();  

insert into LiveMeals (
[MealName] ,        
[ChefID]    ,       
[ChefName]   ,      
[MealPrice]   ,     
[MealLocation] ,    
[MealTime]      ,   
[dietary_Meat]   ,  
[dietary_Shellfish],
[dietary_Fish]     ,
[dietary_Gluten]   ,
[dietary_Nut]      ,
[dietary_Dairy]    ,
[dietary_Egg]      ,
[MealSpaces]       ,
[Description]      ,
[MealType]         ,
[MealStatus]       
)
SELECT MealName,
	ChefID,
	(SELECT UserName FROM AspNetUsers WHERE TempTable.ChefID = AspNetUsers.Id) as ChefName,
	MealPrice,
	MealLocation,
	MealTime,
	dietary_Meat,
	dietary_Shellfish,
	dietary_Fish,
	dietary_Gluten,
	dietary_Nut,
	dietary_Dairy,
	dietary_Egg,
	MealSpaces,
	[Description],
	(SELECT CASE
        WHEN DATEPART(HOUR,MealTime) < 11 AND MealPrice > 2.5 THEN 'Breakfast'
        WHEN DATEPART(HOUR,MealTime) < 12 AND MealPrice > 2.5 THEN 'Brunch'
		WHEN DATEPART(HOUR,MealTime) < 14 AND MealPrice > 2.5 THEN 'Lunch'
		WHEN DATEPART(HOUR,MealTime) < 18 AND MealPrice > 2.5 THEN 'Main'
		WHEN DATEPART(HOUR,MealTime) < 20 AND MealPrice > 2.5 THEN 'Dinner'
		WHEN DATEPART(HOUR,MealTime) < 22 AND MealPrice > 2.5 THEN 'Dessert'
		ELSE 'Snack'
       END) as MealType,
	   MealStatus
FROM
(SELECT (SELECT TOP 1 MealName FROM LiveMealExamples ORDER BY newid()) as MealName,
	(SELECT TOP 1 Id FROM AspNetUsers ORDER BY newid()) as ChefID,
	('blank') as ChefName,
	(SELECT TOP 1 MealPrice FROM LiveMealExamples ORDER BY newid()) as MealPrice,
	(SELECT TOP 1 MealLocation FROM LiveMealExamples ORDER BY newid()) as MealLocation,
	(SELECT DATEADD(HOUR,RAND()*(22-9)+9,
		DATEADD(MINUTE,RAND()*(60-1)+1,
		DATEADD(d, DATEDIFF(d,0, @smalldatetime)+1, 0)))) as MealTime,
	(SELECT TOP 1 dietary_Meat FROM LiveMealExamples ORDER BY newid()) as dietary_Meat,
	(SELECT TOP 1 dietary_Shellfish FROM LiveMealExamples ORDER BY newid()) as dietary_Shellfish,
	(SELECT TOP 1 dietary_Fish FROM LiveMealExamples ORDER BY newid()) as dietary_Fish,
	(SELECT TOP 1 dietary_Gluten FROM LiveMealExamples ORDER BY newid()) as dietary_Gluten,
	(SELECT TOP 1 dietary_Nut FROM LiveMealExamples ORDER BY newid()) as dietary_Nut,
	(SELECT TOP 1 dietary_Dairy FROM LiveMealExamples ORDER BY newid()) as dietary_Dairy,
	(SELECT TOP 1 dietary_Egg FROM LiveMealExamples ORDER BY newid()) as dietary_Egg,
	(SELECT TOP 1 MealSpaces FROM LiveMealExamples ORDER BY newid()) as MealSpaces,
	(SELECT TOP 1 [Description] FROM LiveMealExamples ORDER BY newid()) as [Description],
	('Snack') as MealType, 
	('Live') as MealStatus) as TempTable



 --select * from AspNetUsers