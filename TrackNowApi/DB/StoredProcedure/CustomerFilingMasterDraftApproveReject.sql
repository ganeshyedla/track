/****** Object:  StoredProcedure [dbo].[CustomerFilingMasterDraftApproveReject]    Script Date: 31-03-2023 22:11:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[CustomerFilingMasterDraftApproveReject]
@WorkflowId Numeric(18,0),
@LoginUserdID Numeric(18,0),
@ApprovedOrRejected Varchar(10)
as
Begin
	Set NoCount on
	BEGIN TRY 
		Begin Transaction
		Declare @DraftId Numeric(18,0)

		Update	CustomerFilingMasterWorkflow set @DraftId =DraftId,  WorkflowStatus=@ApprovedOrRejected, 
				UpdateDate=GetDate(), UpdateUser=@LoginUserdID
				where WorkflowId = @WorkflowId

		If @ApprovedOrRejected='Approved'
		Begin
			Insert into	CustomerFilingMasterHistory
			select	CustomerID, FilingID, Notes, BusinessOperation, 'Approval Screen', Getdate(), @LoginUserdID, Null,Null
			from	CustomerFilingMasterDraft

			Delete a from CustomerFilingMaster a Inner Join CustomerFilingMasterDraft d
						on a.FilingID=d.FilingID and a.CustomerId=d.CustomerId 
			where d.DraftID = @DraftId and d.BusinessOperation like '%Delete%'

			Insert Into CustomerFilingMaster
			Select CustomerId, FilingID, Notes, GETDATE(), @LoginUserdID, Null,Null
			from CustomerFilingMasterDraft d
			where d.DraftID = @DraftId and d.BusinessOperation like '%Add%'
		End
		COMMIT TRANSACTION
	End Try
	Begin Catch
		RollBack TRANSACTION
	End Catch
	Set NoCount off
End
GO

