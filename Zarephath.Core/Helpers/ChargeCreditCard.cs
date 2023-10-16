using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using System;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Helpers
{
    public class ChargeCreditCard
    {
        public static TranscationResponceVM Run(PaymentVM paymentVM, String ApiLoginID, String ApiTransactionKey)
        {
            TranscationResponceVM transcationResponce = new TranscationResponceVM();
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            //ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var creditCard = new creditCardType
            {
                cardNumber = paymentVM.CardNumber,
                expirationDate = paymentVM.EXPIRYMM + paymentVM.EXPIRYYY,
                cardCode = paymentVM.CCV
            };

            var billingAddress = new customerAddressType
            {
                company = paymentVM.CompanyName,
                firstName = paymentVM.FirstName,
                lastName = paymentVM.LastName,
                phoneNumber = paymentVM.PhoneNumber,
                address = paymentVM.Address,
                zip = paymentVM.ZipCode,
                email = paymentVM.Email
                
            };
            var shippingAddress = new customerAddressType
            {
                company = paymentVM.CompanyName,
                firstName = paymentVM.FirstName,
                lastName = paymentVM.LastName,
                phoneNumber = paymentVM.PhoneNumber,
                address = paymentVM.Address,
                zip = paymentVM.ZipCode,
                email = paymentVM.Email
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = creditCard };

            // Add line Items
            var lineItems = new lineItemType[1];
            lineItems[0] = new lineItemType { itemId = Crypto.Decrypt(paymentVM.InvoiceNumber), name = paymentVM.BillingMonthDate, quantity = 1, unitPrice = Convert.ToDecimal(Crypto.Decrypt(paymentVM.Amount)) };
            //lineItems[1] = new lineItemType { itemId = "2", name = "snowboard", quantity = 1, unitPrice = new Decimal(450.00) };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card
                amount = Convert.ToDecimal(Crypto.Decrypt(paymentVM.Amount)),
                payment = paymentType,
                billTo = billingAddress,
                lineItems = lineItems,
                shipTo = shippingAddress


            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the controller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            // validate response
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if (response.transactionResponse.messages != null)
                    {
                        transcationResponce.TransactionId = response.transactionResponse.transId;
                        transcationResponce.ResponseCode = response.messages.message[0].code;
                        transcationResponce.Text = response.messages.message[0].text;
                        transcationResponce.Code = response.transactionResponse.messages[0].code;
                        transcationResponce.Description = response.transactionResponse.messages[0].description;
                        transcationResponce.AuthCode = response.transactionResponse.authCode;
                    }
                    else
                    {
                        //Failed Transaction
                        if (response.transactionResponse.errors != null)
                        {
                            transcationResponce.ErrorCode = response.transactionResponse.errors[0].errorCode;
                            transcationResponce.ErrorText = response.transactionResponse.errors[0].errorText;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Failed Transaction.");
                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        transcationResponce.ErrorCode = response.transactionResponse.errors[0].errorCode;
                        transcationResponce.ErrorText = response.transactionResponse.errors[0].errorText;
                    }
                    else
                    {
                        transcationResponce.Code = response.messages.message[0].code;
                        transcationResponce.Text = response.transactionResponse.errors[0].errorText;
                    }
                }
            }
            else
            {
                transcationResponce = null;
            }

            return transcationResponce;
        }

        //private string CheckCardType()
        //{
        //    TypeCardType
        //    return "";
        //}
    }

    public class PaymentModel
    {

        #region APIDetails
        public string ApiKey { get; set; }
        public string TransactionKey { get; set; }
        #endregion

        #region CompanyDetails
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        #endregion

        #region CardDetails
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiredDate { get; set; }
        public string ExpiredYear { get; set; }
        public string CVVNumber { get; set; }
        public decimal Amount { get; set; }

        #endregion



        #region CustomerAddress
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        #endregion
    }
}
