SELECT Amt.Id, Text, Path, Filename 
FROM dbo.AttachmentTable as Amt
LEFT JOIN dbo.AttachmentTypeTable as AmtType
ON Amt.TypeId=AmtType.Id
WHERE PersonalInfoId = 0;