namespace OPM.SFS.Web.SharedCode
{
    public class CommitmentProcessConst
    {
        public const string Incomplete = "1"; //Value = Incomplete
        public const string ApprovalPendingPI = "2"; //Value = PIApprovalPending
        public const string ApprovalPendingPO = "5"; //Value = POApprovalPending
        public const string RequestFinalDocs = "17"; //Value = PORequestFOLPosDesc
        public const string FinalDocsPendingApproval = "19"; //Value = POApprovalPendingFOLPosDesc
        public const string Approved = "15"; //Value = POApproved
        public const string Rejected = "7"; //Value = POReject

        public const string CommitmentApprovalTentative = "Tentative";
        public const string CommitmentApprovalFinal = "Final";

        public const string CommitmentDocumentTentative = "TentativeOffer";
        public const string CommitmentDocumentFinal = "FinalOffer";
        public const string CommitmentDocumentPositionDescription = "PositionDesc";

        //Commitment Type
        public const string CommitmentTypeInternship = "I";
        public const string CommitmentTypePostGrad = "P";


    }
}
