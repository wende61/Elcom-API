using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Http;
using Elcom.Common;
using Elcom.Core;
using Elcom.DataObjects;

namespace Elcom.Api
{
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IAuthorizationService<UserAuthorizationService> _authorizationUserService;
        private readonly IAuthorizationService<ClientAuthorizationService> _authorizationServiceClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepositoryBase<User> _userRepository;
        public AuthorizationAttribute(IAuthorizationService<UserAuthorizationService> authorizationUserService,
            IAuthorizationService<ClientAuthorizationService> authorizationServiceClient,
            IRepositoryBase<User> userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _authorizationUserService = authorizationUserService;
            _authorizationServiceClient = authorizationServiceClient;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public void EasyPass(AuthorizationFilterContext context, string actionController)
        {
            if (actionController != "Token-GetToken")
            {
                var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(authHeader))
                {
                    var clientToken = authHeader.Replace("Bearer ", "");
                    var clientClaims = _authorizationServiceClient.GetClaim(clientToken);
                    
                    if(clientClaims != null && clientClaims.Count() > 0)
                    {
                        if (!AllowAnonymousClient.Contains(actionController))
                        {
                            if (!AllowAnonymous.Contains(actionController))
                            {
                                var tokenUser = context.HttpContext.Request.Headers["UserToken"].ToString();
                                tokenUser = tokenUser.Replace("Bearer ", "");
                                if (!(string.IsNullOrEmpty(tokenUser)) && !(string.IsNullOrWhiteSpace(tokenUser)))
                                {
                                    var claimsUser = _authorizationUserService.GetClaim(tokenUser);
                                    if (!(claimsUser != null && claimsUser.Count() > 0))
                                    {
                                        context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                                    }
                                    var username = claimsUser.Where(u => u.Type == Keys.JWT_CURRENT_USER_CLAIM).FirstOrDefault();
                                    _httpContextAccessor.HttpContext.Session.SetString("CurrentUsername", username.Value);
                                }
                                else
                                    context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                            }
                        }
                    }
                    else
                        context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                }
                else
                    context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
            }
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //first check the client App authentication and then check individual user Authentication
            //All request must send a token as client app
            //for individual user authentication there are requests to allow Anonymous
            if (context != null && context?.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                string actionController = String.Format("{0}-{1}", descriptor.ControllerName, descriptor.ActionName);
                EasyPass(context, actionController);

                //if (!AllowAnonymousClient.Contains(actionController))
                //{
                //    var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
                //    if (!string.IsNullOrEmpty(authHeader))
                //    {
                //        var clientToken = authHeader.Replace("Bearer ", "");
                //        var clientClaims = _authorizationServiceClient.GetClaim(clientToken);
                //        if (clientClaims != null && clientClaims.Count() > 0)
                //        {
                //            var clientUsername = clientClaims.Where(u => u.Type == Keys.JWT_CLIENT_CURRENT_USER_CLAIM).FirstOrDefault();
                //            if (clientUsername != null)
                //            {
                //                var isClientAuthenticated = _authorizationServiceClient.IsAuthenticated(clientToken);
                //                if (isClientAuthenticated == true)
                //                {
                //                    var isClientAuthorized = _authorizationServiceClient.IsAuthorized(clientUsername.Value, actionController);

                //                    if (isClientAuthorized.Status != OperationStatus.SUCCESS)
                //                        context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                //                    var actionPrivilegies = context.HttpContext.Request.Headers["ActionPrivilegies"].ToString();
                //                    if (string.IsNullOrEmpty(actionPrivilegies) || string.IsNullOrWhiteSpace(actionPrivilegies))
                //                        context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);

                //                    if (!AllowAnonymous.Contains(actionPrivilegies))
                //                    {
                //                        //now check individual 
                //                        var tokenUser = context.HttpContext.Request.Headers["UserToken"].ToString();
                //                        if (!(string.IsNullOrEmpty(tokenUser)) && !(string.IsNullOrWhiteSpace(tokenUser)))
                //                        {
                //                            var claimsUser = _authorizationUserService.GetClaim(tokenUser);
                //                            if (claimsUser != null && claimsUser.Count() > 0)
                //                            {
                //                                var username = claimsUser.Where(u => u.Type == Keys.JWT_CURRENT_USER_CLAIM).FirstOrDefault();
                //                                if (username != null)
                //                                {
                //                                    var isUserAuthenticated = _authorizationUserService.IsAuthenticated(tokenUser);                                                    
                //                                    if (isUserAuthenticated)
                //                                    {
                //                                        _httpContextAccessor.HttpContext.Session.SetString("CurrentUsername", username.Value);
                //                                        var userSubscription = _userRepository.FirstOrDefault(u => u.Email == username.Value, new string[] { nameof(AccountSubscription) });
                //                                        if (userSubscription == null)
                //                                            context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                //                                       // _httpContextAccessor.HttpContext.Session.SetString("AccountSubscriptionId", userSubscription.AccountSubscriptionId.ToString());
                //                                        //check the privilege of the user
                //                                        var isUserAuthorized = _authorizationUserService.IsAuthorized(username.Value, actionPrivilegies);

                //                                        if (isUserAuthorized.Status != OperationStatus.SUCCESS)
                //                                            context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                //                                    }
                //                                    else
                //                                        context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                //                                }
                //                                else
                //                                    context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                //                            }
                //                            else
                //                                context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                //                        }
                //                        else
                //                            context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                //                    }
                //                }
                //                else
                //                    context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                //            }
                //            else
                //                context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                //        }
                //        else
                //            context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                //    }
                //    else
                //        context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
                //}
            }
            else
                context.Result = new CustomUnauthorizedResult(Resources.UnautorizedAccess);
        }

        private readonly List<string> AllowAnonymous = new List<string>
        {//with out user login
            "Password-ForgotPassword",
            "Password-ResetForgotPassword",
            "Account-ConfirmUser",
            "Account-SignIn",
            "Account-Save",
            "Supplier-Create",
            "Project-GetOpenBids",
            "Country-GetAll",
            "VendorType-GetAll",
            "SupplyBusinessCategoryType-GetAll",
            "SupplyBusinessCategory-GetAll",
            "Account-RegisterSupplier",
            //"Account-Save",
            //"Account-Save",
            // "Account-Update",
            // "Account-GetAll",
            // "Account-GetById",
           // "Privilege-Create",
           //  "Privilege-View",
           //    "Role-GetAll",
           //"Role-View",
           // //"Privilege-Save",
           // //"Role-GetAll",
           // "Role-GetById",
           // "Role-Update",
           // "Role-Save"
        };

        private readonly List<string> AllowAnonymousClient = new List<string>
        {                        
            //with out app-login
            "Token-GetToken",
           // "ClientRole-Save",
           // "Streaming-SeedClientPriveledgeDatabase",
           // "Streaming-SeedPriveledgeDatabase",
           // "Streaming-SeedDbClientPriviledgeRoleCombination",
           // "Streaming-SeedDbPriviledgeRoleCombination",
           // "ClientAccount-Save",
           // "Account-SignIn",
           // "Supplier-GetForInvitation",
           // "Supplier-RegisterAsync",
           // "Supplier-Create",
           // "Supplier-Delete",
           // "Supplier-Update",
           // "Supplier-GetAll",
           // "Supplier-GetById",
           // "Supplier-BulkInsertion",
           // "Supplier-Register",
           // "CostCenter-Create",
           // "CostCenter-Delete",
           // "CostCenter-Update",
           // "CostCenter-GetAll",
           // "CostCenter-GetById",
           // "CostCenter-BulkInsertion",
           // "Person-Create",
           // "Person-Delete",
           // "Person-Update",
           // "Person-GetAll",
           // "Person-GetById",
           // "Person-BulkInsertion",
           // "Office-Create",
           // "Office-GetById",
           // "Office-Update",
           // "Office-GetAll",
           // "Office-Delete",
           // "Office-BulkInsertion",
           // "Country-Create",
           // "Country-GetById",
           // "Country-Update",
           // "Country-GetAll",
           // "Country-Delete",
           // "Station-Create",
           // "Station-GetById",
           // "Station-Update",
           // "Station-GetAll",
           // "Station-Delete",
           // "Station-BulkInsertion",
           // "SupplyBusinessCategoryType-Create",
           // "SupplyBusinessCategoryType-GetAll",
           // "SupplyBusinessCategoryType-GetById",
           // "SupplyBusinessCategoryType-Update",
           // "SupplyBusinessCategoryType-Delete",
           // "SupplyBusinessCategory-Create",
           // "SupplyBusinessCategory-GetAll",
           // "SupplyBusinessCategory-GetById",
           // "SupplyBusinessCategory-Update",
           // "SupplyBusinessCategory-Delete",
           // "VendorType-Create",
           // "VendorType-GetById",
           // "VendorType-Update",
           // "VendorType-GetAll",
           // "VendorType-Delete",
           // "PurchaseType-Create",
           // "PurchaseType-GetById",
           // "PurchaseType-Update",
           // "PurchaseType-GetAll",
           // "PurchaseType-Delete",
           //  "PurchaseGroup-Create",
           // "PurchaseGroup-GetById",
           // "PurchaseGroup-Update",
           // "PurchaseGroup-GetAll",
           // "PurchaseGroup-Delete",

           // "RequirmentPeriod-Create",
           // "RequirmentPeriod-GetById",
           // "RequirmentPeriod-Update",
           // "RequirmentPeriod-GetAll",
           // "RequirmentPeriod-GetByPurchaseGroupId",
           // "RequirmentPeriod-Delete",

           // "ProcurementSection-Create",
           // "ProcurementSection-GetById",
           // "ProcurementSection-Update",
           // "ProcurementSection-GetAll",
           // "ProcurementSection-GetByRequirementPeriodId",
           // "ProcurementSection-Delete",

           //  "PurchaseRequisition-Reject",
           //  "PurchaseRequisition-Assign",
           //  "PurchaseRequisition-Create",
           //  "PurchaseRequisition-UnAssign",
           //  "PurchaseRequisition-GetMyPurchaseRequsition",
           //  "PurchaseRequisition-GetMyAssignedPurchaseRequsition",
           //  "PurchaseRequisition-GetAll",
           //  "PurchaseRequisition-GetById",
           //  "HotelAccommodation-UnAssign",
           //  "HotelAccommodation-Reject",
           //  "HotelAccommodation-GetAll",
           //  "HotelAccommodation-Create",
           //  "HotelAccommodation-Assign",
           //  "HotelAccommodation-GetMyPurchaseRequsition",
           //  "HotelAccommodation-GetMyAssignedHotelAccomodation",
           //  "HotelAccommodation-GetById",

           //  "Project-GetInvitationBids",
           //  "Project-ShowBidInterests",
           //  //"Project-ExpressInterestOnInvitationBid",
           //  //"Project-ExpressInterestOnOpenBid",
           //  "Project-DefineBidClosing",
           //  "Project-GetProjectOverview",
           //  "Project-GetById",
           //  "Project-Initiate",
           //  "Project-GetAll",
           //  "Project-GetAllPurchaseProjects",
           //  "Project-GetAllHotelAccommodationProjects",
           //  "Project-GetMyProjects",
           //  "Project-GetMyHotelAccommodationProjects",
           //  "Project-GetMyPurchaseProjects",
           //  "Project-InviteTender",
           //  "Project-GetOpenBids",
           //  "Project-AssignProcessType",
           //  "ProjectTeam-Create",
           //  "ProjectTeam-Delete",
           //  "ProjectTeam-GetByProjectId",
           //  "ProjectTeam-GetById",

           //  "RequestForDocument-Approve",
           //  "RequestForDocument-Create",
           //  "RequestForDocument-GetAll",
           //  "RequestForDocument-GetById",
           //  "RequestForDocument-GetByProjectId",

           //  "TechnicalEvaluation-Create",
           //  "TechnicalEvaluation-GetByParentId",
           //  "TechnicalEvaluation-GetById",
           //  "TechnicalEvaluation-Update",
           //  "TechnicalEvaluation-Delete",

           //  "FinancialEvaluation-Create",
           //  "FinancialEvaluation-GetByParentId",
           //  "FinancialEvaluation-GetById",
           //  "FinancialEvaluation-Update",
           //  "FinancialEvaluation-Delete",

           //   "FinancialCriteriaGroup-Create",
           //  "FinancialCriteriaGroup-GetByParentId",
           //  "FinancialCriteriaGroup-GetById",
           //  "FinancialCriteriaGroup-Update",
           //  "FinancialCriteriaGroup-Delete",

           //   "FinancialCriteria-Create",
           //  "FinancialCriteria-GetByParentId",
           //  "FinancialCriteria-GetById",
           //  "FinancialCriteria-Update",
           //  "FinancialCriteria-Delete",

           //    "CriteriaGroup-Create",
           //  "CriteriaGroup-GetByParentId",
           //  "CriteriaGroup-GetById",
           //  "CriteriaGroup-Update",
           //  "CriteriaGroup-Delete",

           //   "FinancialCriteriaItem-Create",
           //  "FinancialCriteriaItem-GetByParentId",
           //  "FinancialCriteriaItem-GetById",
           //  "FinancialCriteriaItem-Update",
           //  "FinancialCriteriaItem-Delete",

           //  "Criteria-Create",
           //  "Criteria-GetByParentId",
           //  "Criteria-GetById",
           //  "Criteria-Update",
           //  "Criteria-Delete",

           //  "HotelAccommodationCriteria-Create",
           //  "HotelAccommodationCriteria-GetByProjectId",
           //  "HotelAccommodationCriteria-GetById",
           //  "HotelAccommodationCriteria-Update",
           //  "HotelAccommodationCriteria-Delete",


           //  "Privilege-View",
           //  "Account-Save",
           //  "Account-Update",
           //  "Account-GetAll",
           //  "Account-GetById",
           //  "Account-RegisterSupplier",
           // "Privilege-Create",
           // "Privilege-GetAll",
           // "Privilege-Save",
           //"Role-GetAll",
           //"Role-View",
           //"Role-GetById",
           //"Role-Update",
           //"Role-Save",
           //"Streaming-SeedDatabase",
           //"Supplier-Create",
           //"Supplier-Register",
           //"Account-SignIn",
           //"Account-Save",
           //"Project-GetOpenBids"
        };
    }
    public class CustomUnauthorizedResult : JsonResult
    {
        public CustomUnauthorizedResult(string message) : base(new ErrorDetails(message))
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
