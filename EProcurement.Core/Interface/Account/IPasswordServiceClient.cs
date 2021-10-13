using EProcurement.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core
{
	public interface IPasswordServiceClient
	{ 
		Task<OperationStatusResponse> ForgotPassword(ForgotPasswordRequest request);
		Task<RecoverPasswordResponse> ResetForgotPassword(ResetForgotPasswordRequest request);
		Task<RecoverPasswordResponse> VerifyRecoveryToken(string token);
		Task<OperationStatusResponse> ChangePassword(ChangePasswordRequest request);
		Task<OperationStatusResponse> ResetPassword(ResetPasswordRequest request);
	}
}
