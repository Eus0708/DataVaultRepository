DELETE FROM AddressTable
WHERE Id < 51;

DBCC CHECKIDENT('dbo.AddressTable', RESEED, -1);

INSERT INTO AddressTable (Address1, Address2, City, StateId, ZipCode) VALUES 
('747 Sumner Ave.', 'apt. 1', 'Syracuse', '4', '12314'), 
('1535 Sumner Ave.', 'apt. 1', 'Syracuse', '4', '12314'), 
('1233 King Ave.', 'apt. 5', 'Syracuse', '20', '34728'), 
('747 Sumner Ave.', NULL, 'Brooklyn', '4', '34728'), 
('123 King Ave.', 'apt. 1', 'Syracuse', '20', '12313'), 
('1 King Ave.', NULL, 'Boston', '20', '34728'), 
('5345 Sumner Ave.', NULL, 'Syracuse', '4', '634534')