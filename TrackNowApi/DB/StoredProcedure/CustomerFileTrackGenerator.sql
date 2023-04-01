/****** Object:  StoredProcedure [dbo].[CustomerFileTrackGenerator]    Script Date: 31-03-2023 22:10:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--CustomerFileTrackGenerator 'Monthly'
--Go

CREATE procedure [dbo].[CustomerFileTrackGenerator]
@Frequency varchar(100)
as
Begin

	--declare @Frequency varchar(100) = 'Quarterly'
	Declare @CurrentDate Datetime = GetDate(), @YearStartMonth Datetime,@CurrentYear int = Datepart(Year,getdate()), @MonthsInfrquency int,
			@DaysinFrequency int

	select @DaysinFrequency = DaysinFrequency, @MonthsInfrquency = MonthsInfrquency from FilingFrquency where FilingFrquencyName = @Frequency
		
	Set @YearStartMonth = isnull((select '01-'+[ConfigItemValue]+'-'+cast(@CurrentYear as varchar) 
	from [dbo].[AppConfiguration] where Configitem='MonthOftheYear'),'01-Jan'+cast(@CurrentYear as varchar))

	Declare @Tmpfrequency Table (FrequencyNo int, StartDate Datetime, EndDate Datetime, Quarter varchar(5), Frequency varchar(100))

	if (@DaysinFrequency is not null and @DaysinFrequency<>0)
	Begin
		;WITH months(MonthNumber) AS
		(
			SELECT 0
			UNION ALL
			SELECT MonthNumber+@MonthsInfrquency
			FROM months
			WHERE MonthNumber < 366
		)
		Insert into @Tmpfrequency
		select	MonthNumber+@MonthsInfrquency As FrequencyNo,
				CONVERT(varchar,Dateadd(DAY,MonthNumber,@YearStartMonth),1) StartDate,
				CONVERT(varchar,Dateadd(DAY,MonthNumber+@MonthsInfrquency-1,@YearStartMonth),1) as LastDate,
				CASE
				WHEN Datepart(month,Dateadd(DAY,MonthNumber,@YearStartMonth))  BETWEEN 1  AND 3  THEN 'Q1'
				WHEN Datepart(month,Dateadd(DAY,MonthNumber,@YearStartMonth)) BETWEEN 4  AND 6  THEN 'Q2'
				WHEN Datepart(month,Dateadd(DAY,MonthNumber,@YearStartMonth)) BETWEEN 7  AND 9  THEN 'Q3'
				WHEN Datepart(month,Dateadd(DAY,MonthNumber,@YearStartMonth)) BETWEEN 10 AND 12 THEN 'Q4'
			END AS Quarter,@Frequency
		from months
		WHERE Dateadd(DAY,MonthNumber+@MonthsInfrquency-1,@YearStartMonth) <= Dateadd(month,12,@YearStartMonth) 
		option(maxrecursion 500)
	End
	else
	Begin
		;WITH months(MonthNumber) AS
		(
			SELECT 0
			UNION ALL
			SELECT MonthNumber+@MonthsInfrquency
			FROM months
			WHERE MonthNumber < 11
		)
		Insert into @Tmpfrequency
		select	MonthNumber+@MonthsInfrquency as FrequencyNo,
				CONVERT(varchar,Dateadd(month,MonthNumber,DATEADD(month, DATEDIFF(month, 0, @YearStartMonth), 0)),1) StartDate,
				CONVERT(varchar,EOMONTH(@YearStartMonth,MonthNumber+@MonthsInfrquency-1),1) as LastDate,
				CASE
				WHEN MonthNumber+@MonthsInfrquency BETWEEN 1  AND 3  THEN 'Q1'
				WHEN MonthNumber+@MonthsInfrquency BETWEEN 4  AND 6  THEN 'Q2'
				WHEN MonthNumber+@MonthsInfrquency BETWEEN 7  AND 9  THEN 'Q3'
				WHEN MonthNumber+@MonthsInfrquency BETWEEN 10 AND 12 THEN 'Q4'
			END AS Quarter,@Frequency 
		from months
		WHERE MonthNumber < 12
	End
	Insert into CustomerFileTracking
	Select	m.CustomerId, b.FilingID, DATEADD(day, DueDayofFrequency, StartDate) as DueDate, 'Pending',
			Getdate(), 'Scheduler', null, null
	from @Tmpfrequency a 
	Inner Join FilingMaster b on a.Frequency = b.FilingFrequency
	Inner Join CustomerFilingMaster m on m.filingid=b.filingid
	where	getdate() between StartDate and EndDate and 
			not exists (select * from CustomerFileTracking c 
					where c.FilingID=b.FilingID and customerid=m.customerid
						  and c.Duedate between a.startdate and a.enddate)

--	select * from CustomerFileTracking
End
GO

