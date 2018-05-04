SELECT Person.Id,
Name.FirstName, Name.MiddleName, Name.LastName, 
Person.DateOfBirth,
Address.Address1, Address.Address2, Address.City, State.Text, Address.ZipCode,
Phone.AreaCode, Phone.PhoneNumber,
Person.SSN,
Person.DateCreated, Person.DateModified

FROM dbo.PersonalInfoTable as Person
LEFT JOIN dbo.AddressTable as Address
ON Person.AddressId=Address.Id
LEFT JOIN dbo.NameTable as Name
ON Person.NameId=Name.Id
LEFT JOIN dbo.PhoneTable as Phone
ON Person.PhoneId=Phone.Id
LEFT JOIN dbo.StateTable as State
ON Address.StateId=State.Id;