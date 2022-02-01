INSERT INTO Accounts(
	Id,
	FirstName,
	LastName) 
VALUES(
	'2344',
	'Tommy',
	'Test'),
	(
	'8080',
	'Ethan',
	'Empty');
INSERT INTO Readings(
	Id,
	MeterReading_Date,
	MeterReading_Value,
	AccountId)
VALUES(
	'1',
	'2019-04-22 09:24:00',
	'11002',
	'2344');