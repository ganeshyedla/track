/****** Object:  StoredProcedure [dbo].[CustomerFilingMasterDraftUpdate]    Script Date: 31-03-2023 23:00:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[CustomerFilingMasterDraftUpdate]
@JsonCustomerFiling varchar(max),
@LoggedInUser varchar(100)
as
Begin
	
	Declare @TmpCustomerFilingMasterDraftUpdate AS [dbo].[CustomerFilingMasterDraft]
	
	select	(select Max(DraftId) from CustomerFilingMasterDraft)
			+Dense_rank() over( order by ApproverName) Draftid, 
			d.filingid, f.Juristiction,d.customerid, d.notes,d.CreateDate, d.CreateUser, 
			d.UpdateDate, d.UpdateUser, d.Status, d.BusinessOperation, 
			a.ApproverId, a.ApproverName , a.State  
	Into #Temp
	from (SELECT * 
	FROM OPENJSON(@JsonCustomerFiling)
	With(
		[CustomerId] [numeric](18, 0),
		[FilingId] [numeric](18, 0),
		[Notes] [varchar](2000) ,
		[CreateDate] [datetime] ,
		[CreateUser] [varchar](100) ,
		[UpdateDate] [datetime] ,
		[UpdateUser] [varchar](100) ,
		[Status] [varchar](100) ,
		[BusinessOperation] [varchar](100) ,
		[Id] [numeric](18, 0),
		[DraftId] [numeric](18, 0) )) d 
	Inner Join FilingMaster  f on f.filingid = d.filingid --and d.filingid=133
	Left Join Approvers a on d.customerid=a.customerid 
	and 1 = Case when f.Juristiction Like '%Federal%'  and a.state is null then 1
				 when f.Juristiction like '%State%'  and a.state =f.stateinfo then 1 else 0 end
	and a.isdefault=1
	BEGIN TRY 
		Begin Transaction

			--Update #temp set BusinessOperation = 'No Change' 
			--where  exists(select * from CustomerFilingMaster 
			--				 where FilingID = #temp.FilingId and CustomerId=#Temp.Customerid)

			Update #temp set BusinessOperation = 'Filing Added' 
			where  Not exists(select * from CustomerFilingMaster 
							 where FilingID = #temp.FilingId and CustomerId=#Temp.Customerid)

			Update #temp set BusinessOperation = 'Filing Removed' 
			where  Not exists(select * from CustomerFilingMaster 
							 where FilingID = #temp.FilingId and CustomerId=#Temp.Customerid)

			Insert into CustomerFilingMasterDraft
			select Customerid, FilingId, Notes, CreateDate, CreateUser, UpdateDate, UpdateUser, [Status], BusinessOperation, DraftId from #temp

			Insert into CustomerFilingMasterWorkflow
			select Draftid, Getdate(), @LoggedInUser, ApproverId, [Status], Null, Null from #temp
			Group by Draftid, ApproverId, [Status]
		COMMIT TRANSACTION
	End Try
	Begin Catch
		RollBack TRANSACTION
	End Catch

	drop table #temp
End
GO

