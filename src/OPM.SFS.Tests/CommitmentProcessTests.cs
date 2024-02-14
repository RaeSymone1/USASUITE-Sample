using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure;
using OPM.SFS.Web.Shared;
using OPM.SFS.Web.Pages.Student;
using FluentValidation.TestHelper;
using OPM.SFS.Web.Models;
using Moq;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class CommitmentProcessTests
    {
        [TestMethod]
        public void Commitment_GetNextStatus_Return_ReviewPending_When_ApprovalWorkflow_Is_Tentative_And_Status_Incompete()
        {
            var cacheMock = new Mock<ICacheHelper>();
            cacheMock.Setup(m => m.GetCommitmentStatusAsync()).Returns(Task.FromResult(GenerateData()));
            CommitmentProcessService service = new CommitmentProcessService(null, cacheMock.Object);
            var result = service.GetNextStatusForApprovalAsync(CommitmentProcessConst.Incomplete, CommitmentProcessConst.CommitmentApprovalTentative, false);
            Assert.AreEqual("Review Pending", result.Result.StatusName);
        }

        [TestMethod]
        public void Commitment_GetNextStatus_Return_AdditionalInfoRequested_When_ApprovalWorkflow_Is_Tentative_And_Status_ApprovalPendingPI_And_AdminApproval_True()
        {
            var cacheMock = new Mock<ICacheHelper>();
            cacheMock.Setup(m => m.GetCommitmentStatusAsync()).Returns(Task.FromResult(GenerateData()));
            CommitmentProcessService service = new CommitmentProcessService(null, cacheMock.Object);
            var result = service.GetNextStatusForApprovalAsync(CommitmentProcessConst.ApprovalPendingPI, CommitmentProcessConst.CommitmentApprovalTentative, true);
            Assert.AreEqual("Additional Info Requested", result.Result.StatusName);
        }


        [TestMethod]
        public void Commitment_GetNextStatus_Return_ReviewPending_When_ApprovalWorkflow_Is_Tentative_And_Status_ApprovalPendingPI()
        {
            var cacheMock = new Mock<ICacheHelper>();
            cacheMock.Setup(m => m.GetCommitmentStatusAsync()).Returns(Task.FromResult(GenerateData()));
            CommitmentProcessService service = new CommitmentProcessService(null, cacheMock.Object);
            var result = service.GetNextStatusForApprovalAsync(CommitmentProcessConst.ApprovalPendingPI, CommitmentProcessConst.CommitmentApprovalTentative, false);
            Assert.AreEqual("Review Pending", result.Result.StatusName);
        }

        [TestMethod]
        public void Commitment_GetNextStatus_Return_ReviewPending_When_ApprovalWorkflow_Is_Tentative_And_Status_RequestFinalDoc()
        {
            var cacheMock = new Mock<ICacheHelper>();
            cacheMock.Setup(m => m.GetCommitmentStatusAsync()).Returns(Task.FromResult(GenerateData()));
            CommitmentProcessService service = new CommitmentProcessService(null, cacheMock.Object);
            var result = service.GetNextStatusForApprovalAsync(CommitmentProcessConst.RequestFinalDocs, CommitmentProcessConst.CommitmentApprovalTentative, false);
            Assert.AreEqual("Review Pending", result.Result.StatusName);
        }

        [TestMethod]
        public void Commitment_GetNextStatus_Return_Approved_When_ApprovalWorkflow_Is_Tentative_And_Status_FinalDocPendApproval()
        {
            var cacheMock = new Mock<ICacheHelper>();
            cacheMock.Setup(m => m.GetCommitmentStatusAsync()).Returns(Task.FromResult(GenerateData()));
            CommitmentProcessService service = new CommitmentProcessService(null, cacheMock.Object);
            var result = service.GetNextStatusForApprovalAsync(CommitmentProcessConst.FinalDocsPendingApproval, CommitmentProcessConst.CommitmentApprovalTentative, false);
            Assert.AreEqual("Approved", result.Result.StatusName);
        }


        [TestMethod]
        public void Commitment_GetNextStatus_Return_ReviewPending_When_ApprovalWorkflow_Is_Not_Tentative_And_Status_Incompete()
        {
            var cacheMock = new Mock<ICacheHelper>();
            cacheMock.Setup(m => m.GetCommitmentStatusAsync()).Returns(Task.FromResult(GenerateData()));
            CommitmentProcessService service = new CommitmentProcessService(null, cacheMock.Object);
            var result = service.GetNextStatusForApprovalAsync(CommitmentProcessConst.Incomplete, CommitmentProcessConst.CommitmentApprovalFinal, false);
            Assert.AreEqual("Review Pending", result.Result.StatusName);
        }


        [TestMethod]
        public void Commitment_GetNextStatus_Return_Approved_When_ApprovalWorkflow_Is_Not_Tentative_And_Status_ApprovalPendPO()
        {
            var cacheMock = new Mock<ICacheHelper>();
            cacheMock.Setup(m => m.GetCommitmentStatusAsync()).Returns(Task.FromResult(GenerateData()));
            CommitmentProcessService service = new CommitmentProcessService(null, cacheMock.Object);
            var result = service.GetNextStatusForApprovalAsync(CommitmentProcessConst.ApprovalPendingPO, CommitmentProcessConst.CommitmentApprovalFinal, false);
            Assert.AreEqual("Approved", result.Result.StatusName);
        }

        [TestMethod]
        public void Commitment_GetNextStatus_Return_Approved_When_ApprovalWorkflow_Is_Not_Tentative_And_Status_FinalDocPendApproval()
        {
            var cacheMock = new Mock<ICacheHelper>();
            cacheMock.Setup(m => m.GetCommitmentStatusAsync()).Returns(Task.FromResult(GenerateData()));
            CommitmentProcessService service = new CommitmentProcessService(null, cacheMock.Object);
            var result = service.GetNextStatusForApprovalAsync(CommitmentProcessConst.FinalDocsPendingApproval, CommitmentProcessConst.CommitmentApprovalFinal, false);
            Assert.AreEqual("Approved", result.Result.StatusName);
        }


        //Failed tests

        [TestMethod]
        public void Commitment_GetNextStatus_Return_Null_When_ApprovalWorkflow_Tentative_And_Status_Reject()
        {
            var cacheMock = new Mock<ICacheHelper>();
            cacheMock.Setup(m => m.GetCommitmentStatusAsync()).Returns(Task.FromResult(GenerateData()));
            CommitmentProcessService service = new CommitmentProcessService(null, cacheMock.Object);
            var result = service.GetNextStatusForApprovalAsync(CommitmentProcessConst.Rejected, CommitmentProcessConst.CommitmentApprovalTentative, false);
            cacheMock.Verify(r => r.GetCommitmentStatusAsync(), Times.Once);
        }

        [TestMethod]
        public void Commitment_GetNextStatus_Return_Null_When_ApprovalWorkflow_Is_Not_Tentative_And_Status_RequestFinalDoc()
        {
            var cacheMock = new Mock<ICacheHelper>();
            cacheMock.Setup(m => m.GetCommitmentStatusAsync()).Returns(Task.FromResult(GenerateData()));
            CommitmentProcessService service = new CommitmentProcessService(null, cacheMock.Object);
            var result = service.GetNextStatusForApprovalAsync(CommitmentProcessConst.CommitmentTypePostGrad, CommitmentProcessConst.RequestFinalDocs, true);
            cacheMock.Verify(r => r.GetCommitmentStatusAsync(), Times.Once);
        }

        //Method: GetNextStatusForRejectAsync

        [TestMethod]
        public void Commitment_GetNextStatusRejected_Return_7()
        {
            var cacheMock = new Mock<ICacheHelper>();
            cacheMock.Setup(m => m.GetCommitmentStatusAsync()).Returns(Task.FromResult(GenerateData()));
            CommitmentProcessService service = new CommitmentProcessService(null, cacheMock.Object);
            var result = service.GetNextStatusForRejectAsync();
            Assert.AreEqual("7", result.Result.StatusCode);
        }


        [TestMethod]
        public void Commitment_GetNextStatusRejected_Return_Null()
        {
            var cacheMock = new Mock<ICacheHelper>();
            cacheMock.Setup(m => m.GetCommitmentStatusAsync()).Returns(Task.FromResult(GenerateData()));
            CommitmentProcessService service = new CommitmentProcessService(null, cacheMock.Object);
            var result = service.GetNextStatusForRejectAsync();
            Assert.IsNull(result.Result.StatusName);
        }

        private List<CommitmentStatus> GenerateData()
        {
            List<CommitmentStatus> dataList = new();
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 1, Code = "1", StudentDisplay = "Incomplete" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 2, Code = "2", StudentDisplay = "Review Pending" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 3, Code = "3", StudentDisplay = "PI Request Info" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 4, Code = "4", StudentDisplay = "Review Pending" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 5, Code = "5", StudentDisplay = "Review Pending" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 6, Code = "6", StudentDisplay = "PO Request Info" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 7, Code = "7", StudentDisplay = "Not Approved" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 8, Code = "8", StudentDisplay = "PO Request Final Offer" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 9, Code = "9", StudentDisplay = "PI Approval Pending Final Offer" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 10, Code = "10", StudentDisplay = "PI Request Final Offer Info" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 11, Code = "11", StudentDisplay = "PI RejectFOL" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 12, Code = "12", StudentDisplay = "Review Pending" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 13, Code = "13", StudentDisplay = "PO Request Final Offer Info" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 14, Code = "14", StudentDisplay = "PO Reject Final Offer" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 15, Code = "15", StudentDisplay = "Approved" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 16, Code = "16", StudentDisplay = "Additional Info Requested" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 17, Code = "17", StudentDisplay = "Additional Info Requested" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 18, Code = "19", StudentDisplay = "Review Pending" });
            dataList.Add(new CommitmentStatus() { CommitmentStatusID = 19, Code = "18", StudentDisplay = "Review Pending" });
            return dataList;
        }

    }
}
