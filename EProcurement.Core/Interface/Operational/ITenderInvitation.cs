using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EProcurement.Common;
using EProcurement.Common.RequestModel.Operational;
using EProcurement.Common.ResponseModel.Operational;

namespace EProcurement.Core.Interface.Operational
{
    public interface ITenderInvitation<Response,Request>
    {
        Task<Response>Invite(Request request);
        PostedBidsResponse GetPostedBids();
        PostedBidsResponse GetOpenBidsInternal();
        PostedBidsResponse GetInvitationBids();
        Task<OperationStatusResponse> ExpressInterestOnInvitationdBid(BidInterestRequest request);
        Task<OperationStatusResponse> ExpressInterestOnOpenBid(BidInterestRequest request);
        SupplierTenderInvitationResponse ShowBidInterests(long Id);
        Task<SupplierTenderInvitationResponse> ShortListSupplier(ShortListRequest request);
        ShortListedResponses GetShortListedSuppliers(long projectId);
        Task<SupplierTenderInvitationResponse> FloatRequestDocument(FloatRequest request);
        ProposalResponses GetProposals();
        Task<OperationStatusResponse> SubmitProposal(SubmitProposalRequest request);
        SubmitedPropsalResponse GetSubmitedProposal(long Id);
    }
}
