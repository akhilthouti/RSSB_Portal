using Microsoft.AspNetCore.Mvc;
using INDO_FIN_NET.Models;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using INDO_FIN_NET.Repository.Data;
using System.Net;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using INDO_FIN_NET.Controllers.Organisation;
using INDO_FIN_NET.Models.UserDetails;
using iTextSharp.text.pdf;
using System.Net.Mail;
using System.Drawing.Imaging;
using iTextSharp.text;
using System.Text;
using Rotativa.AspNetCore;
using ServiceProvider1.Models;
using ServiceProvider1.Models.OrgExceptionLogs;
using ServiceProvider1.Models.UserDetails;
using INDO_FIN_NET.Models.Organisation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using RestSharp;
using Org.BouncyCastle.Ocsp;
using Nancy.Json;
using System.Configuration;

using Azure.Storage.Blobs;
using static System.Net.WebRequestMethods;
using System.Reflection.Metadata;
using System.ComponentModel;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Blobs.Specialized;
using Spire.Pdf.Xmp;
using Aspose.Pdf;
using System.Globalization;

namespace INDO_FIN_NET.Controllers
{
    public class CustomerAccountController : Controller
    {
        ClsCustAccountFormDetails objAccount = new ClsCustAccountFormDetails();
        TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
        AdmKycCustomerDetail objSaving = new AdmKycCustomerDetail();
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData;
        private readonly IConfiguration configuration;
        private readonly string _connectionString;

        public CustomerAccountController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration_)
        {
            objDetails = Context;
            objData = iNDO_;
            configuration = configuration_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }

        TripleDESImplementation objtriple = new TripleDESImplementation();
        clsAddNewUser objAddUsers = new clsAddNewUser();
        ClsUser objuser = new ClsUser();
        string imgtypePOI = "";
        string imgtypePhoto = "";
        string imgtype_POI = "";
        string imgtype_CA = "";
        byte[] dochistory_SI = null;
        string imgtypeSI = "";
        byte[] dochistory_Photo = null;
        byte[] dochistory_POI = null;
        byte[] dochistory_CA = null;
        byte[] byteDoc = null;
        byte[] byteimg;

        [HttpGet]
        public ActionResult CustomerAccForm([FromServices] IActiveLogin objLogin)//(long? CustomerDetailId)
        {
            ErrorLog error_log = new ErrorLog();
            var agentid = HttpContext.Session.GetString("UseID");
            var CustomerFlow = HttpContext.Session.GetString("CustMobileNo");
            if (CustomerFlow != null)
            {
                ViewBag.CustomerFlow = true;
            }
            else if (agentid != null)
            {
                ViewBag.CustomerFlow = false;
            }

            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                if (ViewBag.CustomerFlow == false)
                {
                    string message = "Session Expired";
                    return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                }
                else if (ViewBag.CustomerFlow == true)
                {
                    return RedirectToAction("CustomerRegistration", "CustomerRegistration");
                }
                else
                {
                    return RedirectToAction("MainHomePage", "OrganisationLogin");
                }
            }
            else if (LoginStatus == null)
            {
                if (ViewBag.CustomerFlow == false)
                {
                    string message = "Session Expired";
                    return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                }
                else if (ViewBag.CustomerFlow == true)
                {
                    return RedirectToAction("CustomerRegistration", "CustomerRegistration");
                }
                else
                {
                    return RedirectToAction("MainHomePage", "OrganisationLogin");
                }
            }
            try
            {
                var result1 = HttpContext.Session.GetString("DAEditCustomerdetailId");
                if (HttpContext.Session.GetString("JointAdmin") != null)
                {
                    ViewBag.jointAdmin = HttpContext.Session.GetString("JointAdmin");
                    HttpContext.Session.SetString("PersonalId", result1);

                }
                var jointAccId = HttpContext.Session.GetString("JointACCdata"); 
                var CustomerId = HttpContext.Session.GetString("PersonalId");
                var UserName = HttpContext.Session.GetString("UserName");
                if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
                {
                    HttpContext.Session.GetString("DAEditCustomerdetailId");
                    ViewBag.AdminFlag = "AdminFlag";
                }
                DateTime Datetime = DateTime.Now;
                var result = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerQEData {CustomerId}").AsEnumerable().FirstOrDefault();
                if (result1 != null || jointAccId != null)
                {
                    if (jointAccId != null)
                    {
                        result1 = CustomerId;
                        ViewBag.JointDATA = "true";
                    }
                    var result3 = objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_Account_GetAccountDetails {result1}").AsEnumerable().FirstOrDefault();
                    objAccount.Branch = result3.Branch;
                    objAccount.BirthPlaceCity = result3.BirthPlaceCity;
                    objAccount.OccupationType = result3.OccupationType;
                    objAccount.ResidentialStatus = result3.ResidentialStatus;
                    objAccount.BusinessFirm = result3.BusinessFirm;
                    objAccount.SalaryEmployed = result3.SalariedEmployed;
                    objAccount.Designation = result3.Designation;
                    objAccount.EducationQualification = result3.EducationQualification;
                    objAccount.AnnualIncome = result3.AnnualIncome;
                    objAccount.ThresholdLimit = result3.ThresholdLimit;
                    objAccount.NatureOfOrganization = result3.NatureOrganisation;
                    objAccount.NatureOfOrganizationRemark = result3.NatureOrganisationOtherRemark;
                    objAccount.initialDepositAmount = result3.InitialDepositAmount;
                    objAccount.Nationality = result3.Nationality;
                    //objAccount.Caste = result.CasteCd;
                    string conn1 = _connectionString;
                    using (SqlConnection connection12 = new SqlConnection(conn1))
                    {
                        SqlCommand cmd12 = new SqlCommand("USP_GetCasteName", connection12);
                        cmd12.CommandType = CommandType.StoredProcedure;

                        cmd12.Parameters.AddWithValue("@Cast_Code", result.CasteCd);
                        connection12.Open();
                        SqlDataReader reader1 = cmd12.ExecuteReader();
                        if (reader1.Read())
                        {


                            var Caste = reader1[2].ToString();
                            objAccount.Caste = Caste;
                        }
                    }
                    objAccount.FatherMName = result3.FatherSpouseMname;

                    objAccount.MotherFName = result3.MotherFname;
                    objAccount.MotherMName = result3.MotherMname;
                    objAccount.MotherLName = result3.MotherLname;
                    objAccount.EducationQualification = result3.EducationQualification;
                    objAccount.DepositNumberOfTxnPerMonth = result3.DepositNumberOfTxnPerMonth;
                    objAccount.WithdrawNumberOfTxnPerMonth = result3.WithdrawNumberOfTxnPerMonth;
                    objAccount.DepositValueOfTxnPerMonth = result3.DepositValueOfTxnPerMonth;
                    objAccount.WithdrawValueOfTxnPerMonth = result3.WithdrawValueOfTxnPerMonth;
                    objAccount.DepositTotalFundsDepositedinThreeMonth = result3.DepositTotalFundsDepositedinThreeMonth;
                    objAccount.WithdrawTotalFundsDepositedinThreeMonth = result3.WithdrawTotalFundsDepositedinThreeMonth;
                    objAccount.DepositTxnPerMonthChequeOrTransfer = result3.DepositTxnPerMonthChequeOrTransfer;
                    objAccount.WithdrawTxnPerMonthChequeOrTransfer = result3.WithdrawTxnPerMonthChequeOrTransfer;

                    objAccount.NomineeFName = result3.NominationFname;
                    objAccount.NomineeMName = result3.NominationMname;
                    objAccount.NomineeLName = result3.NominationLname;
                    objAccount.Nominee_ADDRESS_1 = result3.NominationAddress1;
                    objAccount.Nominee_ADDRESS_2 = result3.NominationAddress2;
                    objAccount.Nominee_ADDRESS_3 = result3.NominationAddress3;
                    objAccount.Nominee_CITY = result3.Nominationcity;
                    objAccount.Nominee_COUNTRY = result3.NominationCountry;
                    objAccount.Nominee_Pincode = result3.NominationPincode;
                    objAccount.Nominee_STATE = result3.NominationState;
                    objAccount.OfficeUseInitialAmount = result3.InitialDepositAmount; //result3.OfficeUseTabInitialAmount;
                    objAccount.OfficeUseDC_NO = result3.InitialDepositChequeNo; //result3.OfficeUseTabDcno;
                    objAccount.OfficeUseDate = Datetime; //result3.OfficeUseTabDate;
                    objAccount.OfficeUseWF_No = result3.OfficeUseTabWfno;
                    objAccount.OfficeUseNameAndSign = result3.CustomerFname + " " + result3.CustomerMname + " " + result3.CustomerLname;// result3.OfficeUseTabOfficerName;
                    objAccount.OfficeUseTicket_No = result3.OfficeUseTabTicketNo;
                    objAccount.AOC_OfficerName = UserName;//result3.AoctabOfficerName;
                    objAccount.AOC_TicketNo = result3.AoctabTicketNo;
                    objAccount.ABCcell_OfficerName = result3.AbctabOfficerName;
                    objAccount.ABCcell_TicketNo = result3.AbctabTicketNo;
                    objAccount.initialDepositChequeNo = result3.InitialDepositChequeNo;
                    objAccount.CosmoRupayCard = Convert.ToBoolean(result3.EbankCosmoRupay);
                    objAccount.CosmoVisaDebitCard = Convert.ToBoolean(result3.EbankCosmoVisa);
                    objAccount.UPI = Convert.ToBoolean(result3.EUPI);
                    objAccount.InternetBanking = Convert.ToBoolean(result3.EInternetBanking);
                    objAccount.IMPS = Convert.ToBoolean(result3.EIMPS);
                    objAccount.CarLoan = Convert.ToBoolean(result3.CarLoan);
                    objAccount.HomeLoan = Convert.ToBoolean(result3.HomeLoan);
                    objAccount.ConsumerLoan = Convert.ToBoolean(result3.ConsumerLoan);
                    objAccount.BusinessLoan = Convert.ToBoolean(result3.BusinessLoan);
                    objAccount.EducationLoan = Convert.ToBoolean(result3.EducationLoan);
                    objAccount.Staff = Convert.ToBoolean(result3.Staff);
                    objAccount.RelativeFriend = Convert.ToBoolean(result3.RelativeOrfriend);
                    objAccount.OtherCreditFacility = Convert.ToBoolean(result3.OtherCredit);
                    objAccount.ckycIdentifier = result3.Profession;
                    objAccount.NomineeAge = Convert.ToString(result3.NomineeAge);
                    objAccount.NomineeRelation = result3.NomineeRelation;
                    objAccount.TDSReason = result3.TDSReason;
                    objAccount.VerifyDOC = result3.VerifyDOC;
                    objAccount.PhysicallyChall = result3.PhysicallyChall;
                    objAccount.PhysicallyChall_YN = result3.PhysicallyChall_YN;

                    //cash or chque
                    if (result3.InitialDepositType == "Cash")
                    {
                        ViewBag.Cash = "true";
                    }
                    else
                    {
                        ViewBag.chque = "true";
                    }
                    //for Account type
                    if (result3.AccountType == "CosmoYouth")
                    {
                        ViewBag.CosmoYouth = "true";
                    }
                    else if (result3.AccountType == "CurrentAccount")
                    {
                        ViewBag.Regular = "true";
                    }
                    else if (result3.AccountType == "SavingAccount")
                    {
                        ViewBag.CosmoPremium = "true";
                    }
                    else if (result3.AccountType == "CosmoSalary")
                    {
                        ViewBag.CosmoSalary = "true";
                    }
                    else if (result3.AccountType == "CosmoRoyale")
                    {
                        ViewBag.CosmoRoyale = "true";
                    }
                    else if (result3.AccountType == "CosmoPremiumPlus")
                    {
                        ViewBag.CosmoPremiumPlus = "true";
                    }
                    else if (result3.AccountType == "BSBDA")
                    {
                        ViewBag.BSBDA = "true";
                    }
                    else if (result3.AccountType == "CosmoPremiumSalary")
                    {
                        ViewBag.CosmoPremiumSalary = "true";
                    }
                    else if (result3.AccountType == "other")
                    {
                        ViewBag.other = "true";
                    }
                    else
                    {
                        ViewBag.Otherr = "false";
                    }
                    //For E-Bank
                    if (result3.EbankCosmoRupay == true)
                    {
                        ViewBag.EbankCosmoRupay = "true";
                    }
                    else if (result3.EUPI == true)
                    {
                        ViewBag.EUPI = "true";
                    }
                    else if (result3.EInternetBanking == true)
                    {
                        ViewBag.EInternetBanking = "true";
                    }
                    else if (result3.EIMPS == true)
                    {
                        ViewBag.EIMPS = "true";
                    }
                    else if (result3.EbankCosmoVisa == true)
                    {
                        ViewBag.EbankCosmoVisa = "true";
                    }
                    else
                    {
                        ViewBag.EbankCosmoVisaa = "false";
                    }
                    //for Existing Credit Facilites IF Any

                    if (result3.CarLoan == true)
                    {
                        ViewBag.CarLoan = "true";
                    }
                    else if (result3.HomeLoan == true)
                    {
                        ViewBag.HomeLoan = "true";
                    }
                    else if (result3.ConsumerLoan == true)
                    {
                        ViewBag.ConsumerLoan = "true";
                    }
                    else if (result3.BusinessLoan == true)
                    {
                        ViewBag.BusinessLoan = "true";
                    }
                    else if (result3.EducationLoan == true)
                    {
                        ViewBag.EducationLoan = "true";
                    }
                    else if (result3.Newspaper == true)
                    {
                        ViewBag.Newspaper = "true";
                    }
                    else if (result3.Staff == true)
                    {
                        ViewBag.Staff = "true";
                    }
                    else if (result3.RelativeOrfriend == true)
                    {
                        ViewBag.RelativeOrfriend = "true";
                    }
                    else if (result3.Advertise == true)
                    {
                        ViewBag.Advertise = "true";
                    }
                    else if (result3.OtherCredit == true)
                    {
                        ViewBag.OtherCredit = "true";
                    }
                    else
                    {
                        ViewBag.OtherCreditt = "false";
                    }
                    //for AOC_VISAorATM
                    if (result3.AoctabVisaoratm == true)
                    {
                        ViewBag.AoctabVisaoratm = "true";
                    }
                    else if (result3.AoctabSms == true)
                    {
                        ViewBag.AoctabSms = "true";
                    }
                    else if (result3.AoctabAcandKyc == true)
                    {
                        ViewBag.AoctabAcandKyc = "true";
                    }
                    else if (result3.AoctabIbcomformationRequest == true)
                    {
                        ViewBag.AoctabIbcomformationRequest = "true";
                    }
                    else if (result3.AoctabChequeBookRequest == true)
                    {
                        ViewBag.AoctabChequeBookRequest = "true";
                    }
                    else if (result3.AoctabCosmoNetComformationRequest == true)
                    {
                        ViewBag.AoctabCosmoNetComformationRequest = "true";
                    }
                    else if (result3.AoctabAutoSweep == true)
                    {
                        ViewBag.AoctabAutoSweep = "true";
                    }
                    else
                    {
                        ViewBag.AoctabAutoSweepp = "false";
                    }
                    //---------- BIND -------------
                    string conn22 = _connectionString;

                    using (SqlConnection connection22 = new SqlConnection(conn22))
                    {
                        using (SqlConnection connection24 = new SqlConnection(conn22))
                        {
                            SqlCommand cmd24 = new SqlCommand("USP_IDTOSubTitle", connection24);
                            cmd24.CommandType = CommandType.StoredProcedure;

                            cmd24.Parameters.AddWithValue("@NAME_code", result3.NominationPreName);



                            connection24.Open();
                            SqlDataReader reader24 = cmd24.ExecuteReader();
                            if (reader24.Read())
                            {
                                var NomineePrefix = reader24[0].ToString();

                                objAccount.NomineePrefix = NomineePrefix;
                            }


                        }
                    }


                }
                if (result1 != null)
                {


                    var resultforFitrstJoint = objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_GetAccountDetailsforFjoint {result1}").AsEnumerable().FirstOrDefault();
                    if (resultforFitrstJoint != null)
                    {
                        objAccount.firstapplicantFNAME1 = resultforFitrstJoint.CustomerFname;
                        objAccount.firstapplicantMNAME1 = resultforFitrstJoint.CustomerMname;
                        objAccount.firstapplicantLNAME1 = resultforFitrstJoint.CustomerLname;
                        objAccount.MaidenPrefixName1 = resultforFitrstJoint.MaidenPrefix;
                        objAccount.MaidenFName1 = resultforFitrstJoint.MaidenFname;
                        objAccount.MaidenMName1 = resultforFitrstJoint.MaidenMname;
                        objAccount.MaidenLName1 = resultforFitrstJoint.MaidenLname;
                        objAccount.FatherPrefixName1 = resultforFitrstJoint.FatherSpousePrefix;
                        objAccount.FatherFName1 = resultforFitrstJoint.FatherSpouseFname;
                        objAccount.FatherMName1 = resultforFitrstJoint.FatherSpouseMname;
                        objAccount.FatherLName1 = resultforFitrstJoint.FatherSpouseLname;
                        objAccount.MotherPrefixName1 = resultforFitrstJoint.MotherPrefix;

                        objAccount.MotherFName1 = resultforFitrstJoint.MotherFname;
                        objAccount.MotherMName1 = resultforFitrstJoint.MotherMname;
                        objAccount.MotherLName1 = resultforFitrstJoint.MotherLname;


                        objAccount.Branch1 = resultforFitrstJoint.Branch;
                        objAccount.ckycIdentifier1 = resultforFitrstJoint.CkycNumber;
                        objAccount.BirthPlaceCity1 = resultforFitrstJoint.BirthPlaceCity;
                        objAccount.BirthPlaceCountry1 = resultforFitrstJoint.BirthPlaceCountry;
                        objAccount.MaritalStatus1 = resultforFitrstJoint.MaritalStatus;
                        objAccount.Religion1 = resultforFitrstJoint.Religion;
                        objAccount.Caste1 = resultforFitrstJoint.Caste;
                        objAccount.OccupationType1 = resultforFitrstJoint.OccupationType;
                        objAccount.ResidentialStatus1 = resultforFitrstJoint.ResidentialStatus;
                        objAccount.BusinessFirm1 = resultforFitrstJoint.BusinessFirm;
                        objAccount.SalaryEmployed1 = resultforFitrstJoint.SalariedEmployed;
                        objAccount.Designation1 = resultforFitrstJoint.Designation;
                        objAccount.EducationQualification1 = resultforFitrstJoint.EducationQualification;
                        objAccount.AnnualIncome1 = resultforFitrstJoint.AnnualIncome;
                        objAccount.ThresholdLimit1 = resultforFitrstJoint.ThresholdLimit;
                        objAccount.NatureOfOrganization1 = resultforFitrstJoint.NatureOrganisation;
                        objAccount.NatureOfOrganizationRemark1 = resultforFitrstJoint.NatureOrganisationOtherRemark;
                        objAccount.initialDepositAmount1 = resultforFitrstJoint.InitialDepositAmount;
                        objAccount.Nationality1 = resultforFitrstJoint.Nationality;
                        objAccount.initialDepositChequeNo1 = resultforFitrstJoint.InitialDepositChequeNo;





                    }
                    var resultforSecondJoint = objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_GetAccountDetailsforSjoint {result1}").AsEnumerable().FirstOrDefault();
                    if (resultforSecondJoint != null)
                    {
                        ViewBag.SecondJoint = "true";
                        objAccount.SecondapplicantFNAME2 = resultforSecondJoint.CustomerFname;
                        objAccount.SecondapplicantMNAME2 = resultforSecondJoint.CustomerMname;
                        objAccount.SecondapplicantLNAME2 = resultforSecondJoint.CustomerLname;
                        objAccount.MaidenPrefixName2 = resultforSecondJoint.MaidenPrefix;
                        objAccount.MaidenFName2 = resultforSecondJoint.MaidenFname;
                        objAccount.MaidenMName2 = resultforSecondJoint.MaidenMname;
                        objAccount.MaidenLName2 = resultforSecondJoint.MaidenLname;
                        objAccount.FatherPrefixName2 = resultforSecondJoint.FatherSpousePrefix;
                        objAccount.FatherFName2 = resultforSecondJoint.FatherSpouseFname;
                        objAccount.FatherMName2 = resultforSecondJoint.FatherSpouseMname;
                        objAccount.FatherLName2 = resultforSecondJoint.FatherSpouseLname;
                        objAccount.MotherPrefixName2 = resultforSecondJoint.MotherPrefix;

                        objAccount.MotherFName2 = resultforSecondJoint.MotherFname;
                        objAccount.MotherMName2 = resultforSecondJoint.MotherMname;
                        objAccount.MotherLName2 = resultforSecondJoint.MotherLname;


                        objAccount.Branch2 = resultforSecondJoint.Branch;
                        objAccount.ckycIdentifier2 = resultforSecondJoint.CkycNumber;
                        objAccount.BirthPlaceCity2 = resultforSecondJoint.BirthPlaceCity;
                        objAccount.BirthPlaceCountry2 = resultforSecondJoint.BirthPlaceCountry;
                        objAccount.MaritalStatus2 = resultforSecondJoint.MaritalStatus;
                        objAccount.Religion2 = resultforSecondJoint.Religion;
                        objAccount.Caste2 = resultforSecondJoint.Caste;
                        objAccount.OccupationType2 = resultforSecondJoint.OccupationType;
                        objAccount.ResidentialStatus2 = resultforSecondJoint.ResidentialStatus;
                        objAccount.BusinessFirm2 = resultforSecondJoint.BusinessFirm;
                        objAccount.SalaryEmployed2 = resultforSecondJoint.SalariedEmployed;
                        objAccount.Designation2 = resultforSecondJoint.Designation;
                        objAccount.EducationQualification2 = resultforSecondJoint.EducationQualification;
                        objAccount.AnnualIncome2 = resultforSecondJoint.AnnualIncome;
                        objAccount.ThresholdLimit2 = resultforSecondJoint.ThresholdLimit;
                        objAccount.NatureOfOrganization2 = resultforSecondJoint.NatureOrganisation;
                        objAccount.NatureOfOrganizationRemark2 = resultforSecondJoint.NatureOrganisationOtherRemark;
                        objAccount.initialDepositAmount2 = resultforSecondJoint.InitialDepositAmount;
                        objAccount.Nationality2 = resultforSecondJoint.Nationality;
                        objAccount.initialDepositChequeNo2 = resultforSecondJoint.InitialDepositChequeNo;




                    }
                }

                //For maritalstatus
                string conn = _connectionString;
                using (SqlConnection connection11 = new SqlConnection(conn))
                {
                    SqlCommand cmd11 = new SqlCommand("USP_IDTOMaritalstatus", connection11);
                    cmd11.CommandType = CommandType.StoredProcedure;

                    cmd11.Parameters.AddWithValue("@maritalstatus_code", result.maritalstatus);



                    connection11.Open();
                    SqlDataReader reader = cmd11.ExecuteReader();
                    if (reader.Read())
                    {
                        var maritalstatus_desc = reader[0].ToString();
                    }
                    objSaving.FirstName = result.FirstName;
                    objSaving.MiddleName = result.MiddleName;
                    objSaving.LastName = result.LastName;


                    objSaving.ClientAddress1 = result.ClientAddress1;
                    objSaving.ClientAddress2 = result.ClientAddress2;
                    objSaving.ClientAddress3 = result.ClientAddress3;
                    objSaving.Country = result.Country;
                    objSaving.CustomerId = result.CustomerId;
                    objSaving.State = result.State;


                    //For Religion
                    string conn1 = _connectionString;
                    using (SqlConnection connection12 = new SqlConnection(conn1))
                    {
                        SqlCommand cmd12 = new SqlCommand("USP_IDTOReligion", connection12);
                        cmd12.CommandType = CommandType.StoredProcedure;

                        cmd12.Parameters.AddWithValue("@Religion_code", result.Religion);
                        connection12.Open();
                        SqlDataReader reader1 = cmd12.ExecuteReader();
                        if (reader1.Read())
                        {


                            var Religion_description = reader1[0].ToString();
                        }
                        //string conn3 = _connectionString;
                        //using (SqlConnection connection14 = new SqlConnection(conn3))
                        //{
                        //    SqlCommand cmd14 = new SqlCommand("USP_IDTOSubTitle", connection14);
                        //    cmd14.CommandType = CommandType.StoredProcedure;

                        //    cmd14.Parameters.AddWithValue("@NAME_code", result.SubTitle);
                        //    connection14.Open();
                        //    SqlDataReader reader3 = cmd14.ExecuteReader();
                        //    if (reader3.Read())
                        //    {

                        //        var NAME_description = reader3[0].ToString();
                        //    }
                            string conn4 = _connectionString;
                            using (SqlConnection connection15 = new SqlConnection(conn4))
                            {
                                SqlCommand cmd15 = new SqlCommand("USP_IDTOCOUNTRY", connection15);
                                cmd15.CommandType = CommandType.StoredProcedure;

                                cmd15.Parameters.AddWithValue("@Country_Code", result.CountryId);

                                connection15.Open();
                                SqlDataReader reader4 = cmd15.ExecuteReader();
                                if (reader4.Read())
                                {

                                    var Country = reader4[0].ToString();
                                }

                                objAccount.MainHolderapplicantFNAME = result.FirstName;
                                objAccount.MainHolderapplicantMNAME = result.MiddleName;
                                objAccount.MainHolderapplicantLNAME = result.LastName;
                                objAccount.MaidenPrefixName = result.SubTitle;
                                objAccount.MaidenFName = result.FirstName;
                                objAccount.MaidenMName = result.MiddleName;
                                objAccount.MaidenLName = result.LastName;
                                objAccount.FatherFName = result.MiddleName;
                                objAccount.FatherLName = result.LastName;

                                objAccount.PANCardNumber = result.PanNo;
                                objAccount.VoterIdCardNumber = result.VoterId;
                                objAccount.UIDNumber = result.AadharNo;
                                //objAccount.Caste = reader2[0].ToString();
                                objAccount.BirthPlaceCountry = reader4[0].ToString();
                                objAccount.Religion = reader1[0].ToString();
                                objAccount.MaritalStatus = reader[0].ToString();
                                objAccount.Religion = reader1[0].ToString();
                                if (objAccount.BirthPlaceCountry == "INDIA")
                                {
                                    objAccount.Nationality = "INDIAN";
                                }
                                //ViewBag.VerifyData = new SelectList(new[] { new { ID = "P", Value = "Passport" }, new { ID = "V", Value = "Voter ID" }, new { ID = "P1", Value = "Pan" }, new { ID = "DL", Value = "Driving Licence" }, new { ID = "A", Value = "Aadhar" } }, "ID", "Value");
                                ViewBag.VerifyData = new SelectList(new[] { new { ID = "P", Value = "Passport" }, new { ID = "V", Value = "Voter ID" }, new { ID = "P1", Value = "Pan" }, new { ID = "DL", Value = "Driving Licence" }, new { ID = "A", Value = "Aadhar" } }, "ID", "Value");

    
                                var verificationtype = (from details in objDetails.admBranchDetailsS.FromSqlRaw($"USP_GetAllBranchDetailsRSSB").ToList()
                                                        select new SelectListItem()
                                                        {
                                                            Text = details.branch_description.ToString(),
                                                            Value = details.branch_code.ToString(),

                                                        }).ToList();
                                verificationtype.Insert(0, new SelectListItem()
                                {
                                    Text = "----Select----",
                                    Value = string.Empty
                                });
                                ViewBag.BranchDetail = verificationtype;

                                var verificationtype6 = (from details in objDetails.admnames.FromSqlRaw($"USP_Get_Name").ToList()
                                                         select new SelectListItem()
                                                         {
                                                             Text = details.NAME_description.ToString(),
                                                             Value = details.NAME_code.ToString(),

                                                         }).ToList();
                                verificationtype6.Insert(0, new SelectListItem()
                                {
                                    Text = "----Select----",
                                    Value = string.Empty
                                });
                                ViewBag.per_name = verificationtype6;
                                ViewBag.per_name1 = verificationtype6;
                                ViewBag.per_name2 = verificationtype6;

                                ViewBag.ResidentialStatusDetail = new SelectList(new[] { new { ID = "Resident Individual", Value = "Resident Individual" }, new { ID = "Non Resident Indian", Value = "Non Resident Indian" }, new { ID = "Foreign National", Value = "Foreign National" }, new { ID = "Person of Indian Origin", Value = "Person of Indian Origin" } }, "ID", "Value", objAccount.ResidentialStatus);

                                ViewBag.OccupationTypeDetail = new SelectList(new[] { new { ID = "Upto 1 Lac", Value = "Upto 1 Lac" }, new { ID = "1Lac to 3Lac", Value = "1Lac to 3Lac" }, new { ID = "3LAc to 5LAc", Value = "3LAc to 5LAc" }, new { ID = "5Lac to 10Lac", Value = "5Lac to 10Lac" }, new { ID = "10Lac to 20Lac", Value = "10Lac to 20Lac" }, new { ID = "20Lac to 30Lac", Value = "20Lac to 30Lac" }, new { ID = "Above 30Lac", Value = "Above 30Lac" } }, "ID", "Value", objAccount.AnnualIncome);//verificationtype; // verificationtype2;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       //});
                                ViewBag.community = new SelectList(new[] { new { ID = "Upto 1 Lac", Value = "Upto 1 Lac" }, new { ID = "1Lac to 3Lac", Value = "1Lac to 3Lac" }, new { ID = "3LAc to 5LAc", Value = "3LAc to 5LAc" }, new { ID = "5Lac to 10Lac", Value = "5Lac to 10Lac" }, new { ID = "10Lac to 20Lac", Value = "10Lac to 20Lac" }, new { ID = "20Lac to 30Lac", Value = "20Lac to 30Lac" }, new { ID = "Above 30Lac", Value = "Above 30Lac" } }, "ID", "Value", objAccount.AnnualIncome);//verificationtype; //verificationtype3;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //ViewBag.community = new SelectList(objDetails.AdmCosmosMasterData.FromSqlRaw($"USP_Cosmos_GetMasterData {"community"},{"ref_code"}, {"ref_desc"},{objAccount.Religion}"));
                                ViewBag.AnnualIncomeDetail = new SelectList(new[] { new { ID = "Upto 1 Lac", Value = "Upto 1 Lac" }, new { ID = "1Lac to 3Lac", Value = "1Lac to 3Lac" }, new { ID = "3LAc to 5LAc", Value = "3LAc to 5LAc" }, new { ID = "5Lac to 10Lac", Value = "5Lac to 10Lac" }, new { ID = "10Lac to 20Lac", Value = "10Lac to 20Lac" }, new { ID = "20Lac to 30Lac", Value = "20Lac to 30Lac" }, new { ID = "Above 30Lac", Value = "Above 30Lac" } }, "ID", "Value", objAccount.AnnualIncome);
                                ViewBag.NatureOfOrganistion = new SelectList(new[] { new { ID = "Proprietory", Value = "Proprietory" }, new { ID = "Partnership", Value = "Partnership" }, new { ID = "Unlisted Co.", Value = "Unlisted Co." }, new { ID = "Listed Co.", Value = "Listed Co." }, new { ID = "MNC'S", Value = "MNC'S" }, new { ID = "Publc/Govt.", Value = "Publc/Govt." }, new { ID = "Other", Value = "Other" } }, "ID", "Value", objAccount.NatureOfOrganization);
                                //ViewBag.State = new SelectList(objDetails.StateCodes.FromSqlRaw($"USP_Get_State {"State_Code"},{"State_Name"},{objAccount.Nominee_STATE}")).ToList();
                                var verificationtype4 = (from details in objDetails.StateCodes.FromSqlRaw($"USP_Get_State").ToList()
                                                         select new SelectListItem()
                                                         {
                                                             Text = details.StateName,
                                                             Value = details.StateCode1
                                                         }).ToList();
                                verificationtype4.Insert(0, new SelectListItem()
                                {
                                    Text = "----Select----",
                                    Value = string.Empty
                                });
                                ViewBag.State = verificationtype4;
                                var verificationtype5 = (from details in objDetails.MasterCountries.FromSqlRaw($"USP_GET_COUNTRY_DATA").ToList()
                                                         select new SelectListItem()
                                                         {
                                                             Text = details.Country,
                                                             Value = details.CountryCode
                                                         }).ToList();
                                verificationtype5.Insert(0, new SelectListItem()
                                {
                                    Text = "----Select----",
                                    Value = string.Empty
                                });
                                ViewBag.getCountry = verificationtype5;
                                var verificationtype87 = (from details in objDetails.adm_OccupationDetailss.FromSqlRaw($"USP_Get_OccupationData ").ToList()
                                                          select new SelectListItem()
                                                          {
                                                              Text = details.Occupation_desc.ToString(),
                                                              Value = details.Occupation_Code.ToString(),
                                                          }).ToList();
                                verificationtype87.Insert(0, new SelectListItem()
                                {
                                    Text = "----Select----",
                                    Value = string.Empty
                                });
                                ViewBag.GetOccupation = verificationtype87;
                                var verificationtype14 = (from details in objDetails.adm_PhysicallyChallengeds.FromSqlRaw($"USP_Get_PHYSICALLYCHALDetails").ToList()
                                                          select new SelectListItem()
                                                          {
                                                              Text = details.PhysicalChalDesc.ToString(),
                                                              Value = details.PhysicalCha_Code.ToString(),

                                                          }).ToList();
                                verificationtype14.Insert(0, new SelectListItem()
                                {
                                    Text = "----Select----",
                                    Value = string.Empty
                                });
                                ViewBag.PhysicallyChallenged = verificationtype14;
                                var ProfessionType = (from details in objDetails.adm_ProfessionDetailss.FromSqlRaw($"USP_Get_ProfessionData ").ToList()
                                                           select new SelectListItem()
                                                           {
                                                               Text = details.Pro_Value.ToString(),
                                                               Value = details.Pro_Code.ToString(),
                                                           }).ToList();
                                ProfessionType.Insert(0, new SelectListItem()
                                {
                                    Text = "----Select----",
                                    Value = string.Empty
                                });
                                ViewBag.GetProfession = ProfessionType;
                                
                                var verificationtype16 = (from details in objDetails.adm_TDSReasonCodes.FromSqlRaw($"USP_Get_TDSReasonDetails").ToList()
                                                          select new SelectListItem()
                                                          {
                                                              Text = details.TDSReason_Desc.ToString(),
                                                              Value = details.TDSReason_Code.ToString(),

                                                          }).ToList();
                                verificationtype16.Insert(0, new SelectListItem()
                                {
                                    Text = "----Select----",
                                    Value = string.Empty
                                });
                                ViewBag.TDSReason = verificationtype16;
                                ViewBag.PhysicallyChal_YN = new SelectList(new[] { new { ID = "Y", Value = "Y" }, new { ID = "N", Value = "N" } }, "ID", "Value");
                                return View(objAccount);
                            }
                        
                    }



                }
            }


            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        [HttpPost]

        public ActionResult CustomerAccForm([FromServices] IActiveLogin objLogin, ClsCustAccountFormDetails objAccount, string check, string JointID, string IDtype, string Joint1, string UPIType, string InternetBankingType, string IMPSType)
        {
            ErrorLog error_log = new ErrorLog();
            var agentid = HttpContext.Session.GetString("UseID");
            var CustomerFlow = HttpContext.Session.GetString("CustMobileNo");
            if (CustomerFlow != null)
            {
                ViewBag.CustomerFlow = true;
            }
            else if (agentid != null)
            {
                ViewBag.CustomerFlow = false;
            }

            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                if (ViewBag.CustomerFlow == false)
                {
                    string message = "Session Expired";
                    return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                }
                else if (ViewBag.CustomerFlow == true)
                {
                    return RedirectToAction("CustomerRegistration", "CustomerRegistration");
                }
                else
                {
                    return RedirectToAction("MainHomePage", "OrganisationLogin");
                }
            }
            else if (LoginStatus == null)
            {
                if (ViewBag.CustomerFlow == false)
                {
                    string message = "Session Expired";
                    return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                }
                else if (ViewBag.CustomerFlow == true)
                {
                    return RedirectToAction("CustomerRegistration", "CustomerRegistration");
                }
                else
                {
                    return RedirectToAction("MainHomePage", "OrganisationLogin");
                }
            }
            try
            {
                string AccountForm = "";
                //var AccountForm = "Account";
                HttpContext.Session.SetString("Account", AccountForm);
                long? custID = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                var result = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerQEData {custID}").AsEnumerable().FirstOrDefault();
                if (check == "SavePersonal")
                {
                    string AccountType = "";
                    string AccountNumber = "";
                    string AccountopenDate = "";
                    string DepositType = string.Empty;

                    if (objAccount.initialDepositCash == true)
                    {
                        DepositType = "Cash";
                    }
                    if (objAccount.initialDepositCheque == true)
                    {
                        DepositType = "Cheque";
                    }
                    string dobStr = result.Dob;
                    DateTime dob = DateTime.ParseExact(dobStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    //string formattedDob = dob.ToString("yyyy-MM-dd");
                    var minor = IsMinor(dob);
                    var major = Is18OrAbove(dob);
                    if (minor)
                    {
                        objAccount.Minor = "Y";
                    }
                    else if (major)
                    {
                        objAccount.Minor = "N";
                    }
                    else
                    {
                        objAccount.Minor = "N";
                    }
                    using (SqlConnection cn = new SqlConnection(_connectionString))
                    {
                        cn.Open();
                        SqlCommand cmd1 = new SqlCommand("USP_Account_PesonalDetails", cn);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@CustomerID", custID);
                        cmd1.Parameters.AddWithValue("@JointHolderCount", 0);
                        cmd1.Parameters.AddWithValue("@IsJointHolder", objAccount.JointForAccountYes);
                        cmd1.Parameters.AddWithValue("@Branch", objAccount.Branch);
                        //cmd1.Parameters.AddWithValue("@CustomerPrefix", objAccount.CustomerPrefix);
                        cmd1.Parameters.AddWithValue("@AccountType", AccountType);
                        cmd1.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                        cmd1.Parameters.AddWithValue("@AccountopenDate", AccountopenDate);
                        //cmd1.Parameters.AddWithValue("@CkycNumber", objAccount.ckycIdentifier);
                        cmd1.Parameters.AddWithValue("@CustomerFName", objAccount.MainHolderapplicantFNAME);
                        cmd1.Parameters.AddWithValue("@CustomerMName", objAccount.MainHolderapplicantMNAME);
                        cmd1.Parameters.AddWithValue("@CustomerLName", objAccount.MainHolderapplicantLNAME);
                        cmd1.Parameters.AddWithValue("@MaidenPrefix", objAccount.MaidenPrefixName);
                        cmd1.Parameters.AddWithValue("@MaidenFName", objAccount.MaidenFName);
                        cmd1.Parameters.AddWithValue("@MaidenMName", objAccount.MaidenMName);
                        cmd1.Parameters.AddWithValue("@MaidenLName", objAccount.MaidenLName);
                        cmd1.Parameters.AddWithValue("@FatherSpousePrefix", "MR");
                        cmd1.Parameters.AddWithValue("@FatherSpouseFName", objAccount.FatherFName);
                        cmd1.Parameters.AddWithValue("@FatherSpouseMName", objAccount.FatherMName);
                        cmd1.Parameters.AddWithValue("@FatherSpouseLName", objAccount.FatherLName);
                        cmd1.Parameters.AddWithValue("@MotherPrefix", "MRS");
                        cmd1.Parameters.AddWithValue("@MotherFName", objAccount.MotherFName);
                        cmd1.Parameters.AddWithValue("@MotherMName", objAccount.MotherMName);
                        cmd1.Parameters.AddWithValue("@MotherLName", objAccount.MotherLName);
                        cmd1.Parameters.AddWithValue("@BirthPlaceCity", objAccount.BirthPlaceCity);
                        cmd1.Parameters.AddWithValue("@BirthPlaceCountry", objAccount.BirthPlaceCountry);
                        cmd1.Parameters.AddWithValue("@MaritalStatus", objAccount.MaritalStatus);
                        cmd1.Parameters.AddWithValue("@Nationality", objAccount.Nationality);
                        cmd1.Parameters.AddWithValue("@ResidentialStatus", objAccount.ResidentialStatus);
                        cmd1.Parameters.AddWithValue("@Religion", objAccount.Religion);
                        cmd1.Parameters.AddWithValue("@Caste", objAccount.Caste);
                        cmd1.Parameters.AddWithValue("@OccupationType", objAccount.OccupationType);
                        cmd1.Parameters.AddWithValue("@BusinessFirm", objAccount.BusinessFirm);
                        cmd1.Parameters.AddWithValue("@SalariedEmployed", objAccount.SalaryEmployed);
                        cmd1.Parameters.AddWithValue("@Designation", objAccount.Designation);
                        cmd1.Parameters.AddWithValue("@AnnualIncome", objAccount.AnnualIncome);
                        cmd1.Parameters.AddWithValue("@NatureOrganisation", objAccount.NatureOfOrganization);
                        cmd1.Parameters.AddWithValue("@NatureOrganisationOtherRemark", objAccount.NatureOfOrganizationRemark);
                        cmd1.Parameters.AddWithValue("@OperationInstruction", "");
                        cmd1.Parameters.AddWithValue("@InitialDepositType", DepositType);
                        cmd1.Parameters.AddWithValue("@InitialDepositDate", DateTime.Now);
                        cmd1.Parameters.AddWithValue("@InitialDepositAmount", objAccount.initialDepositAmount);
                        cmd1.Parameters.AddWithValue("@InitialDepositChequeNo", "");
                        cmd1.Parameters.AddWithValue("@EducationQualification", objAccount.EducationQualification);
                        cmd1.Parameters.AddWithValue("@ThresholdLimit", objAccount.ThresholdLimit);
                        cmd1.Parameters.AddWithValue("@PrimaryCustomerID", "");
                        cmd1.Parameters.AddWithValue("@Profession", objAccount.ckycIdentifier);
                        cmd1.Parameters.AddWithValue("@PhysicallyChall_YN", objAccount.PhysicallyChall_YN);
                        cmd1.Parameters.AddWithValue("@PhysicallyChall", objAccount.PhysicallyChall);
                        cmd1.Parameters.AddWithValue("@TDSReason", objAccount.TDSReason);
                        cmd1.Parameters.AddWithValue("@VerifyDOC", "Aadhar/Pan");
                        cmd1.Parameters.AddWithValue("@CustDOB", result.Dob);
                        cmd1.Parameters.AddWithValue("@Minor", objAccount.Minor);
                        cmd1.ExecuteNonQuery();

                        return Json("Success");


                    }
                }
                else if (check == "SaveJoint")
                {




                    var Jointresult = (from c in objDetails.AdmCosmosCustomerDetails where c.CustomerId == custID select c).FirstOrDefault();
                    if (Jointresult != null)
                    {
                        if (Jointresult.JointHolderCount == 0)
                        {
                            string AccountType = "";
                            string AccountNumber = "";
                            string AccountopenDate = "";
                            int JointHolderCount = 1;
                            int IsJointHolder = 0;
                            string DepositType = string.Empty;
                            var jointcustID = HttpContext.Session.GetString("CustJoint");
                            if (objAccount.initialDepositCash1 == true)
                            {
                                DepositType = "Cash";
                            }
                            if (objAccount.initialDepositCheque1 == true)
                            {
                                DepositType = "Cheque";
                            }
                            using (SqlConnection con1 = new SqlConnection(_connectionString))
                            {
                                con1.Open();
                                SqlCommand cmd2 = new SqlCommand("USP_Account_PesonalDetails", con1);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@CustomerID", jointcustID);
                                cmd2.Parameters.AddWithValue("@JointHolderCount", JointHolderCount);
                                cmd2.Parameters.AddWithValue("@IsJointHolder", IsJointHolder);

                                cmd2.Parameters.AddWithValue("@Branch", objAccount.Branch1);
                                //cmd1.Parameters.AddWithValue("@CustomerPrefix", objAccount.CustomerPrefix);
                                cmd2.Parameters.AddWithValue("@AccountType", AccountType);
                                cmd2.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                                cmd2.Parameters.AddWithValue("@AccountopenDate", AccountopenDate);
                                //cmd2.Parameters.AddWithValue("@CkycNumber", objAccount.ckycIdentifier1);
                                cmd2.Parameters.AddWithValue("@CustomerFName", objAccount.firstapplicantFNAME1);
                                cmd2.Parameters.AddWithValue("@CustomerMName", objAccount.firstapplicantMNAME1);
                                cmd2.Parameters.AddWithValue("@CustomerLName", objAccount.firstapplicantLNAME1);
                                cmd2.Parameters.AddWithValue("@MaidenPrefix", objAccount.MaidenPrefixName1);
                                cmd2.Parameters.AddWithValue("@MaidenFName", objAccount.MaidenFName1);
                                cmd2.Parameters.AddWithValue("@MaidenMName", objAccount.MaidenMName1);
                                cmd2.Parameters.AddWithValue("@MaidenLName", objAccount.MaidenLName1);
                                cmd2.Parameters.AddWithValue("@FatherSpousePrefix", "MR");
                                cmd2.Parameters.AddWithValue("@FatherSpouseFName", objAccount.FatherFName1);
                                cmd2.Parameters.AddWithValue("@FatherSpouseMName", objAccount.FatherMName1);
                                cmd2.Parameters.AddWithValue("@FatherSpouseLName", objAccount.FatherLName1);
                                cmd2.Parameters.AddWithValue("@MotherPrefix", "MRS");
                                cmd2.Parameters.AddWithValue("@MotherFName", objAccount.MotherFName1);
                                cmd2.Parameters.AddWithValue("@MotherMName", objAccount.MotherMName1);
                                cmd2.Parameters.AddWithValue("@MotherLName", objAccount.MotherLName1);
                                cmd2.Parameters.AddWithValue("@BirthPlaceCity", objAccount.BirthPlaceCity1);
                                cmd2.Parameters.AddWithValue("@BirthPlaceCountry", objAccount.BirthPlaceCountry1);
                                cmd2.Parameters.AddWithValue("@MaritalStatus", objAccount.MaritalStatus1);
                                cmd2.Parameters.AddWithValue("@Nationality", objAccount.Nationality1);
                                cmd2.Parameters.AddWithValue("@ResidentialStatus", objAccount.ResidentialStatus1);
                                cmd2.Parameters.AddWithValue("@Religion", objAccount.Religion1);
                                cmd2.Parameters.AddWithValue("@Caste", objAccount.Caste1);
                                cmd2.Parameters.AddWithValue("@OccupationType", objAccount.OccupationType1);
                                cmd2.Parameters.AddWithValue("@BusinessFirm", objAccount.BusinessFirm1);
                                cmd2.Parameters.AddWithValue("@SalariedEmployed", objAccount.SalaryEmployed1);
                                cmd2.Parameters.AddWithValue("@Designation", objAccount.Designation1);
                                cmd2.Parameters.AddWithValue("@AnnualIncome", objAccount.AnnualIncome1);
                                cmd2.Parameters.AddWithValue("@NatureOrganisation", objAccount.NatureOfOrganization1);
                                cmd2.Parameters.AddWithValue("@NatureOrganisationOtherRemark", objAccount.NatureOfOrganizationRemark1);
                                cmd2.Parameters.AddWithValue("@OperationInstruction", "");
                                cmd2.Parameters.AddWithValue("@InitialDepositType", DepositType);
                                cmd2.Parameters.AddWithValue("@InitialDepositDate", DateTime.Now);
                                cmd2.Parameters.AddWithValue("@InitialDepositAmount", objAccount.initialDepositAmount1);
                                cmd2.Parameters.AddWithValue("@InitialDepositChequeNo", "");
                                cmd2.Parameters.AddWithValue("@EducationQualification", objAccount.EducationQualification1);
                                cmd2.Parameters.AddWithValue("@ThresholdLimit", objAccount.ThresholdLimit1);
                                cmd2.Parameters.AddWithValue("@PrimaryCustomerID", custID);

                                cmd2.ExecuteNonQuery();
                                using (SqlConnection con20 = new SqlConnection(_connectionString))
                                {
                                    con20.Open();
                                    SqlCommand cmd20 = new SqlCommand("USP_ToUpdateJointHolder", con20);
                                    cmd20.CommandType = CommandType.StoredProcedure;
                                    cmd20.Parameters.AddWithValue("@CustomerID", custID);

                                    cmd20.ExecuteNonQuery();
                                    return Json("Success");
                                }

                            }
                        }
                    }
                }
                else if (check == "SaveJoint1")
                {

                    string AccountType = "";
                    string AccountNumber = "";
                    string AccountopenDate = "";
                    string DepositType = string.Empty;
                    int JointHolderCount = 2;
                    int IsJointHolder = 0;

                    var jointcustID = HttpContext.Session.GetString("CustJoint");
                    using (SqlConnection con2 = new SqlConnection(_connectionString))

                    {
                        {

                            con2.Open();
                            SqlCommand cmd3 = new SqlCommand("USP_Account_PesonalDetails", con2);
                            cmd3.CommandType = CommandType.StoredProcedure;
                            cmd3.Parameters.AddWithValue("@CustomerID", jointcustID);
                            cmd3.Parameters.AddWithValue("@JointHolderCount", JointHolderCount);
                            cmd3.Parameters.AddWithValue("@IsJointHolder", IsJointHolder);

                            cmd3.Parameters.AddWithValue("@Branch", objAccount.Branch2);
                            cmd3.Parameters.AddWithValue("@AccountType", AccountType);
                            cmd3.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                            cmd3.Parameters.AddWithValue("@AccountopenDate", AccountopenDate);
                            //cmd3.Parameters.AddWithValue("@CkycNumber", objAccount.ckycIdentifier2);
                            cmd3.Parameters.AddWithValue("@CustomerFName", objAccount.SecondapplicantFNAME2);
                            cmd3.Parameters.AddWithValue("@CustomerMName", objAccount.SecondapplicantMNAME2);
                            cmd3.Parameters.AddWithValue("@CustomerLName", objAccount.SecondapplicantLNAME2);
                            cmd3.Parameters.AddWithValue("@MaidenPrefix", objAccount.MaidenPrefixName2);
                            cmd3.Parameters.AddWithValue("@MaidenFName", objAccount.MaidenFName2);
                            cmd3.Parameters.AddWithValue("@MaidenMName", objAccount.MaidenMName2);
                            cmd3.Parameters.AddWithValue("@MaidenLName", objAccount.MaidenLName2);
                            cmd3.Parameters.AddWithValue("@FatherSpousePrefix", "MR");
                            cmd3.Parameters.AddWithValue("@FatherSpouseFName", objAccount.FatherFName2);
                            cmd3.Parameters.AddWithValue("@FatherSpouseMName", objAccount.FatherMName2);
                            cmd3.Parameters.AddWithValue("@FatherSpouseLName", objAccount.FatherLName2);
                            cmd3.Parameters.AddWithValue("@MotherPrefix", "MRS");
                            cmd3.Parameters.AddWithValue("@MotherFName", objAccount.MotherFName2);
                            cmd3.Parameters.AddWithValue("@MotherMName", objAccount.MotherMName2);
                            cmd3.Parameters.AddWithValue("@MotherLName", objAccount.MotherLName2);
                            cmd3.Parameters.AddWithValue("@BirthPlaceCity", objAccount.BirthPlaceCity2);
                            cmd3.Parameters.AddWithValue("@BirthPlaceCountry", objAccount.BirthPlaceCountry2);
                            cmd3.Parameters.AddWithValue("@MaritalStatus", objAccount.MaritalStatus2);
                            cmd3.Parameters.AddWithValue("@Nationality", objAccount.Nationality2);
                            cmd3.Parameters.AddWithValue("@ResidentialStatus", objAccount.ResidentialStatus2);
                            cmd3.Parameters.AddWithValue("@Religion", objAccount.Religion2);
                            cmd3.Parameters.AddWithValue("@Caste", objAccount.Caste2);
                            cmd3.Parameters.AddWithValue("@OccupationType", objAccount.OccupationType2);
                            cmd3.Parameters.AddWithValue("@BusinessFirm", objAccount.BusinessFirm2);
                            cmd3.Parameters.AddWithValue("@SalariedEmployed", objAccount.SalaryEmployed2);
                            cmd3.Parameters.AddWithValue("@Designation", objAccount.Designation2);
                            cmd3.Parameters.AddWithValue("@AnnualIncome", objAccount.AnnualIncome2);
                            cmd3.Parameters.AddWithValue("@NatureOrganisation", objAccount.NatureOfOrganization2);
                            cmd3.Parameters.AddWithValue("@NatureOrganisationOtherRemark", objAccount.NatureOfOrganizationRemark2);
                            cmd3.Parameters.AddWithValue("@OperationInstruction", "");
                            cmd3.Parameters.AddWithValue("@InitialDepositType", DepositType);
                            cmd3.Parameters.AddWithValue("@InitialDepositDate", DateTime.Now);
                            cmd3.Parameters.AddWithValue("@InitialDepositAmount", objAccount.initialDepositAmount2);
                            cmd3.Parameters.AddWithValue("@InitialDepositChequeNo", "");
                            cmd3.Parameters.AddWithValue("@EducationQualification", objAccount.EducationQualification2);
                            cmd3.Parameters.AddWithValue("@ThresholdLimit", objAccount.ThresholdLimit2);
                            cmd3.Parameters.AddWithValue("@PrimaryCustomerID", custID);
                            cmd3.ExecuteNonQuery();
                            using (SqlConnection con20 = new SqlConnection(_connectionString))
                            {
                                con20.Open();
                                SqlCommand cmd20 = new SqlCommand("USP_ToUpdateJointHolder1", con20);
                                cmd20.CommandType = CommandType.StoredProcedure;
                                cmd20.Parameters.AddWithValue("@CustomerID", custID);

                                cmd20.ExecuteNonQuery();
                                return Json("Success");
                            }
                        }
                    }

                }
                else if (check == "SaveJoint2")

                {
                    string AccountType = "";
                    string AccountNumber = "";
                    string AccountopenDate = "";
                    string DepositType = string.Empty;
                    int JointHolderCount = 3;
                    int IsJointHolder = 3;
                    int IsJointHolder2 = 3;
                    var jointcustID = HttpContext.Session.GetString("CustJoint");
                    using (SqlConnection con15 = new SqlConnection(_connectionString))
                    {
                        con15.Open();
                        SqlCommand cmd23 = new SqlCommand("USP_JointHolderCount", con15);
                        cmd23.CommandType = CommandType.StoredProcedure;
                        cmd23.Parameters.AddWithValue("@CustomerID", custID);
                        cmd23.Parameters.AddWithValue("@JointHolderCount", IsJointHolder);
                        cmd23.Parameters.AddWithValue("@IsJointHolder", IsJointHolder);
                        cmd23.ExecuteNonQuery();




                        using (SqlConnection con4 = new SqlConnection(_connectionString))

                        {

                            {
                                con4.Open();
                                SqlCommand cmd4 = new SqlCommand("USP_Account_PesonalDetails", con4);
                                cmd4.CommandType = CommandType.StoredProcedure;
                                cmd4.Parameters.AddWithValue("@CustomerID", jointcustID);
                                cmd4.Parameters.AddWithValue("@JointHolderCount", JointHolderCount);
                                cmd4.Parameters.AddWithValue("@IsJointHolder", IsJointHolder2);

                                cmd4.Parameters.AddWithValue("@Branch", objAccount.Branch3);
                                //cmd3.Parameters.AddWithValue("@CustomerPrefix", objAccount.CustomerPrefix);
                                cmd4.Parameters.AddWithValue("@AccountType", AccountType);
                                cmd4.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                                cmd4.Parameters.AddWithValue("@AccountopenDate", AccountopenDate);
                                //cmd4.Parameters.AddWithValue("@CkycNumber", objAccount.ckycIdentifier3);
                                cmd4.Parameters.AddWithValue("@CustomerFName", objAccount.ThirdapplicantFNAME3);
                                cmd4.Parameters.AddWithValue("@CustomerMName", objAccount.ThirdapplicantMNAME3);
                                cmd4.Parameters.AddWithValue("@CustomerLName", objAccount.ThirdapplicantLNAME3);
                                cmd4.Parameters.AddWithValue("@MaidenPrefix", objAccount.MaidenPrefixName3);
                                cmd4.Parameters.AddWithValue("@MaidenFName", objAccount.MaidenFName3);
                                cmd4.Parameters.AddWithValue("@MaidenMName", objAccount.MaidenMName3);
                                cmd4.Parameters.AddWithValue("@MaidenLName", objAccount.MaidenLName3);
                                cmd4.Parameters.AddWithValue("@FatherSpousePrefix", "MR");
                                cmd4.Parameters.AddWithValue("@FatherSpouseFName", objAccount.FatherFName3);
                                cmd4.Parameters.AddWithValue("@FatherSpouseMName", objAccount.FatherMName3);
                                cmd4.Parameters.AddWithValue("@FatherSpouseLName", objAccount.FatherLName3);
                                cmd4.Parameters.AddWithValue("@MotherPrefix", "MRS");
                                cmd4.Parameters.AddWithValue("@MotherFName", objAccount.MotherFName3);
                                cmd4.Parameters.AddWithValue("@MotherMName", objAccount.MotherMName3);
                                cmd4.Parameters.AddWithValue("@MotherLName", objAccount.MotherLName3);
                                cmd4.Parameters.AddWithValue("@BirthPlaceCity", objAccount.BirthPlaceCity3);
                                cmd4.Parameters.AddWithValue("@BirthPlaceCountry", objAccount.BirthPlaceCountry3);
                                cmd4.Parameters.AddWithValue("@MaritalStatus", objAccount.MaritalStatus3);
                                cmd4.Parameters.AddWithValue("@Nationality", objAccount.Nationality3);
                                cmd4.Parameters.AddWithValue("@ResidentialStatus", objAccount.ResidentialStatus3);
                                cmd4.Parameters.AddWithValue("@Religion", objAccount.Religion3);
                                cmd4.Parameters.AddWithValue("@Caste", objAccount.Caste3);
                                cmd4.Parameters.AddWithValue("@OccupationType", objAccount.OccupationType3);
                                cmd4.Parameters.AddWithValue("@BusinessFirm", objAccount.BusinessFirm3);
                                cmd4.Parameters.AddWithValue("@SalariedEmployed", objAccount.SalaryEmployed3);
                                cmd4.Parameters.AddWithValue("@Designation", objAccount.Designation3);
                                cmd4.Parameters.AddWithValue("@AnnualIncome", objAccount.AnnualIncome3);
                                cmd4.Parameters.AddWithValue("@NatureOrganisation", objAccount.NatureOfOrganization3);
                                cmd4.Parameters.AddWithValue("@NatureOrganisationOtherRemark", objAccount.NatureOfOrganizationRemark3);
                                cmd4.Parameters.AddWithValue("@OperationInstruction", "");
                                cmd4.Parameters.AddWithValue("@InitialDepositType", DepositType);
                                cmd4.Parameters.AddWithValue("@InitialDepositDate", DateTime.Now);
                                cmd4.Parameters.AddWithValue("@InitialDepositAmount", objAccount.initialDepositAmount3);
                                cmd4.Parameters.AddWithValue("@InitialDepositChequeNo", "");
                                cmd4.Parameters.AddWithValue("@EducationQualification", objAccount.EducationQualification3);
                                cmd4.Parameters.AddWithValue("@ThresholdLimit", objAccount.ThresholdLimit3);
                                cmd4.Parameters.AddWithValue("@PrimaryCustomerID", custID);
                                cmd4.ExecuteNonQuery();
                                return Json("Success");
                            }
                        }
                    }
                }


                else if (check == "SaveJoint3")
                {
                    string AccountType = "";
                    string AccountNumber = "";
                    string AccountopenDate = "";
                    string DepositType = string.Empty;
                    int JointHolderCount = 4;
                    int IsJointHolder = 4;
                    int IsJointHolder3 = 4;
                    var jointcustID = HttpContext.Session.GetString("CustJoint");
                    using (SqlConnection con16 = new SqlConnection(_connectionString))
                    {
                        con16.Open();
                        SqlCommand cmd24 = new SqlCommand("USP_JointHolderCount", con16);
                        cmd24.CommandType = CommandType.StoredProcedure;
                        cmd24.Parameters.AddWithValue("@CustomerID", custID);
                        cmd24.Parameters.AddWithValue("@JointHolderCount", IsJointHolder);
                        cmd24.Parameters.AddWithValue("@IsJointHolder", IsJointHolder);
                        cmd24.ExecuteNonQuery();





                        using (SqlConnection con5 = new SqlConnection(_connectionString))
                        {


                            {

                                con5.Open();
                                SqlCommand cmd5 = new SqlCommand("USP_Account_PesonalDetails", con5);
                                cmd5.CommandType = CommandType.StoredProcedure;
                                cmd5.Parameters.AddWithValue("@CustomerID", jointcustID);
                                cmd5.Parameters.AddWithValue("@JointHolderCount", JointHolderCount);
                                cmd5.Parameters.AddWithValue("@IsJointHolder", IsJointHolder3);

                                cmd5.Parameters.AddWithValue("@Branch", objAccount.Branch4);
                                //cmd4.Parameters.AddWithValue("@CustomerPrefix", objAccount.CustomerPrefix);
                                cmd5.Parameters.AddWithValue("@AccountType", AccountType);
                                cmd5.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                                cmd5.Parameters.AddWithValue("@AccountopenDate", AccountopenDate);
                                //cmd5.Parameters.AddWithValue("@CkycNumber", objAccount.ckycIdentifier4);
                                cmd5.Parameters.AddWithValue("@CustomerFName", objAccount.FourthapplicantFNAME4);
                                cmd5.Parameters.AddWithValue("@CustomerMName", objAccount.FourthapplicantMNAME4);
                                cmd5.Parameters.AddWithValue("@CustomerLName", objAccount.FourthapplicantLNAME4);
                                cmd5.Parameters.AddWithValue("@MaidenPrefix", objAccount.MaidenPrefixName4);
                                cmd5.Parameters.AddWithValue("@MaidenFName", objAccount.MaidenFName4);
                                cmd5.Parameters.AddWithValue("@MaidenMName", objAccount.MaidenMName4);
                                cmd5.Parameters.AddWithValue("@MaidenLName", objAccount.MaidenLName4);
                                cmd5.Parameters.AddWithValue("@FatherSpousePrefix", "MR");
                                cmd5.Parameters.AddWithValue("@FatherSpouseFName", objAccount.FatherFName4);
                                cmd5.Parameters.AddWithValue("@FatherSpouseMName", objAccount.FatherMName4);
                                cmd5.Parameters.AddWithValue("@FatherSpouseLName", objAccount.FatherLName4);
                                cmd5.Parameters.AddWithValue("@MotherPrefix", "MRS");
                                cmd5.Parameters.AddWithValue("@MotherFName", objAccount.MotherFName4);
                                cmd5.Parameters.AddWithValue("@MotherMName", objAccount.MotherMName4);
                                cmd5.Parameters.AddWithValue("@MotherLName", objAccount.MotherLName4);
                                cmd5.Parameters.AddWithValue("@BirthPlaceCity", objAccount.BirthPlaceCity4);
                                cmd5.Parameters.AddWithValue("@BirthPlaceCountry", objAccount.BirthPlaceCountry4);
                                cmd5.Parameters.AddWithValue("@MaritalStatus", objAccount.MaritalStatus4);
                                cmd5.Parameters.AddWithValue("@Nationality", objAccount.Nationality4);
                                cmd5.Parameters.AddWithValue("@ResidentialStatus", objAccount.ResidentialStatus4);
                                cmd5.Parameters.AddWithValue("@Religion", objAccount.Religion4);
                                cmd5.Parameters.AddWithValue("@Caste", objAccount.Caste4);
                                cmd5.Parameters.AddWithValue("@OccupationType", objAccount.OccupationType4);
                                cmd5.Parameters.AddWithValue("@BusinessFirm", objAccount.BusinessFirm4);
                                cmd5.Parameters.AddWithValue("@SalariedEmployed", objAccount.SalaryEmployed4);
                                cmd5.Parameters.AddWithValue("@Designation", objAccount.Designation4);
                                cmd5.Parameters.AddWithValue("@AnnualIncome", objAccount.AnnualIncome4);
                                cmd5.Parameters.AddWithValue("@NatureOrganisation", objAccount.NatureOfOrganization4);
                                cmd5.Parameters.AddWithValue("@NatureOrganisationOtherRemark", objAccount.NatureOfOrganizationRemark4);
                                cmd5.Parameters.AddWithValue("@OperationInstruction", "");
                                cmd5.Parameters.AddWithValue("@InitialDepositType", DepositType);
                                cmd5.Parameters.AddWithValue("@InitialDepositDate", DateTime.Now);
                                cmd5.Parameters.AddWithValue("@InitialDepositAmount", objAccount.initialDepositAmount4);
                                cmd5.Parameters.AddWithValue("@InitialDepositChequeNo", "");
                                cmd5.Parameters.AddWithValue("@EducationQualification", objAccount.EducationQualification4);
                                cmd5.Parameters.AddWithValue("@ThresholdLimit", objAccount.ThresholdLimit4);
                                cmd5.Parameters.AddWithValue("@PrimaryCustomerID", custID);
                                cmd5.ExecuteNonQuery();
                                return Json("Success");
                            }
                        }

                    }


                    return Json("Success");
                }


                else if (check == "SaveAccount")
                {
                    string AccType = string.Empty;
                    if (objAccount.Regular == true)
                    {
                        AccType = "CurrentAccount";
                    }
                    if (objAccount.CosmoPremium == true)
                    {
                        AccType = "SavingAccount";
                    }
                    if (objAccount.CosmoSalary == true)
                    {
                        AccType = "CosmoSalary";
                    }
                    if (objAccount.CosmoRoyale == true)
                    {
                        AccType = "CosmoRoyale";
                    }
                    if (objAccount.CosmoPremiumPlus == true)
                    {
                        AccType = "CosmoPremiumPlus";
                    }
                    if (objAccount.BSBDA == true)
                    {
                        AccType = "BSBDA";
                    }
                    if (objAccount.CosmoYouth == true)
                    {
                        AccType = "CosmoYouth";
                    }
                    if (objAccount.CosmoPremiumSalary == true)
                    {
                        AccType = "CosmoPremiumSalary";
                    }
                    if (objAccount.other == true)
                    {
                        AccType = "other";
                    }
                    string conn12 = _connectionString;
                    using (SqlConnection connection = new SqlConnection(conn12))
                    {
                        SqlCommand cmd12 = new SqlCommand("USP_Account_AccountTypeDetails", connection);
                        cmd12.CommandType = CommandType.StoredProcedure;

                        cmd12.Parameters.AddWithValue("@CustomerID", custID);
                        cmd12.Parameters.AddWithValue("@AccountType", AccType);
                        connection.Open();
                        cmd12.ExecuteNonQuery();
                        connection.Close();
                    }
                    return Json("Success");
                }
                else if (check == "SaveEbank")
                {
                    string conn = _connectionString;
                    using (SqlConnection connection2 = new SqlConnection(conn))
                    {
                        SqlCommand cmd2 = new SqlCommand("USP_Account_EbankingServiceDetails", connection2);
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@CustomerID", custID);
                        cmd2.Parameters.AddWithValue("@EBankCosmoRupay", objAccount.CosmoRupayCard);
                        cmd2.Parameters.AddWithValue("@EBankCosmoVisa", objAccount.CosmoVisaDebitCard);
                        cmd2.Parameters.AddWithValue("@EUPI", Convert.ToBoolean(UPIType));
                        cmd2.Parameters.AddWithValue("@EInternetBanking", Convert.ToBoolean(InternetBankingType));
                        cmd2.Parameters.AddWithValue("@EIMPS", Convert.ToBoolean(IMPSType));
                        connection2.Open();
                        cmd2.ExecuteNonQuery();
                        connection2.Close();
                    }
                    return Json("Success");
                }
                else if (check == "SaveCredit")
                {
                    string conn = _connectionString;
                    using (SqlConnection connection2 = new SqlConnection(conn))
                    {
                        SqlCommand cmd22 = new SqlCommand("USP_Account_CreditDetails", connection2);
                        cmd22.CommandType = CommandType.StoredProcedure;
                        cmd22.Parameters.AddWithValue("@CustomerID", custID);
                        cmd22.Parameters.AddWithValue("@CarLoan", objAccount.CarLoan);
                        cmd22.Parameters.AddWithValue("@ConsumerLoan", objAccount.ConsumerLoan);
                        cmd22.Parameters.AddWithValue("@HomeLoan", objAccount.HomeLoan);
                        cmd22.Parameters.AddWithValue("@BusinessLoan", objAccount.BusinessLoan);
                        cmd22.Parameters.AddWithValue("@EducationLoan", objAccount.EducationLoan);
                        cmd22.Parameters.AddWithValue("@otherRemark", "");
                        cmd22.Parameters.AddWithValue("@Newspaper", objAccount.NewsPaper);
                        cmd22.Parameters.AddWithValue("@Staff", objAccount.Staff);
                        cmd22.Parameters.AddWithValue("@RelativeORFriend", objAccount.RelativeFriend);
                        cmd22.Parameters.AddWithValue("@Advertise", objAccount.Advertise);
                        cmd22.Parameters.AddWithValue("@OtherCredit", objAccount.OtherCreditFacility);
                        connection2.Open();
                        cmd22.ExecuteNonQuery();
                        connection2.Close();
                    }
                    return Json("Success");
                }
                else if (check == "SaveAccountUsage")
                {
                    //objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_Account_AccountUsageDetails{custID},{objAccount.DepositNumberOfTxnPerMonth},{objAccount.DepositValueOfTxnPerMonth},{objAccount.DepositTotalFundsDepositedinThreeMonth},{objAccount.DepositTxnPerMonthChequeOrTransfer},{objAccount.WithdrawNumberOfTxnPerMonth},{objAccount.WithdrawValueOfTxnPerMonth},{objAccount.WithdrawTotalFundsDepositedinThreeMonth},{objAccount.WithdrawTxnPerMonthChequeOrTransfer}");
                    string conn = _connectionString;
                    using (SqlConnection connection22 = new SqlConnection(conn))
                    {
                        SqlCommand cmd32 = new SqlCommand("USP_Account_AccountUsageDetails", connection22);
                        cmd32.CommandType = CommandType.StoredProcedure;
                        cmd32.Parameters.AddWithValue("@CustomerID", custID);
                        cmd32.Parameters.AddWithValue("@DepositNumberOfTxnPerMonth", objAccount.DepositNumberOfTxnPerMonth);
                        cmd32.Parameters.AddWithValue("@DepositValueOfTxnPerMonth", objAccount.DepositValueOfTxnPerMonth);
                        cmd32.Parameters.AddWithValue("@DepositTotalFundsDepositedinThreeMonth", objAccount.DepositTotalFundsDepositedinThreeMonth);
                        cmd32.Parameters.AddWithValue("@DepositTxnPerMonthChequeOrTransfer", objAccount.DepositTxnPerMonthChequeOrTransfer);
                        cmd32.Parameters.AddWithValue("@WithdrawNumberOfTxnPerMonth", objAccount.WithdrawNumberOfTxnPerMonth);
                        cmd32.Parameters.AddWithValue("@WithdrawValueOfTxnPerMonth", objAccount.WithdrawValueOfTxnPerMonth);
                        cmd32.Parameters.AddWithValue("@WithdrawTotalFundsDepositedinThreeMonth", objAccount.WithdrawTotalFundsDepositedinThreeMonth);
                        cmd32.Parameters.AddWithValue("@WithdrawTxnPerMonthChequeOrTransfer", objAccount.WithdrawTxnPerMonthChequeOrTransfer);

                        connection22.Open();
                        cmd32.ExecuteNonQuery();
                        connection22.Close();
                    }
                    return Json("Success");
                }
                else if (check == "SaveNominee")
                {
                    bool nominee = false;
                    if (objAccount.NomineeForAccountYes == true)
                    {
                        nominee = true;
                    }
                    if (objAccount.NomineeForAccountNo == true)
                    {
                        nominee = false;
                    }
                    //objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_Account_AccountNomineeDetails{custID},{nominee},{objAccount.NomineePrefix},{objAccount.NomineeFName},{objAccount.NomineeMName},{objAccount.NomineeLName},{objAccount.Nominee_ADDRESS_1},{objAccount.Nominee_ADDRESS_2},{objAccount.Nominee_ADDRESS_3},{objAccount.Nominee_COUNTRY},{objAccount.Nominee_CITY},{objAccount.Nominee_STATE},{objAccount.Nominee_Pincode}");
                    string conn = _connectionString;
                    using (SqlConnection connection = new SqlConnection(conn))
                    {
                        SqlCommand cmd = new SqlCommand("USP_Account_AccountNomineeDetails", connection);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerID", custID);
                        cmd.Parameters.AddWithValue("@NominationYesOrNo", nominee);
                        cmd.Parameters.AddWithValue("@NominationPreName", objAccount.NomineePrefix);
                        cmd.Parameters.AddWithValue("@NominationFName", objAccount.NomineeFName);
                        cmd.Parameters.AddWithValue("@NominationMName", objAccount.NomineeMName);
                        cmd.Parameters.AddWithValue("@NominationLName", objAccount.NomineeLName);
                        cmd.Parameters.AddWithValue("@NominationAge", objAccount.NomineeAge);
                        cmd.Parameters.AddWithValue("@NominationRelation", objAccount.NomineeRelation);
                        cmd.Parameters.AddWithValue("@NominationAddress1", objAccount.Nominee_ADDRESS_1);
                        cmd.Parameters.AddWithValue("@NominationAddress2", objAccount.Nominee_ADDRESS_2);
                        cmd.Parameters.AddWithValue("@NominationAddress3", objAccount.Nominee_ADDRESS_3);
                        cmd.Parameters.AddWithValue("@NominationCountry", objAccount.Nominee_COUNTRY);
                        cmd.Parameters.AddWithValue("@Nominationcity", objAccount.Nominee_CITY);
                        cmd.Parameters.AddWithValue("@NominationState", objAccount.Nominee_STATE);
                        cmd.Parameters.AddWithValue("@NominationPincode", objAccount.Nominee_Pincode);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();

                    }
                    return Json("Success");
                }
                else if (check == "SaveOffice")
                {
                    //objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_Account_AccountOfficeUseDetails{custID},{objAccount.OfficeUseInitialAmount},{objAccount.OfficeUseDC_NO},{objAccount.OfficeUseDate},{objAccount.OfficeUseWF_No},{objAccount.OfficeUseNameAndSign},{objAccount.OfficeUseTicket_No},{DateTime.Now},{""}");
                    string conn = _connectionString;
                    using (SqlConnection connection22 = new SqlConnection(conn))
                    {
                        SqlCommand cmd42 = new SqlCommand("USP_Account_AccountOfficeUseDetails", connection22);
                        cmd42.CommandType = CommandType.StoredProcedure;
                        cmd42.Parameters.AddWithValue("@CustomerID", custID);
                        cmd42.Parameters.AddWithValue("@OfficeUseTabInitialAmount", objAccount.OfficeUseInitialAmount);
                        cmd42.Parameters.AddWithValue("@OfficeUseTabDCNo", objAccount.OfficeUseDC_NO);
                        cmd42.Parameters.AddWithValue("@OfficeUseTabDate", objAccount.OfficeUseDate);
                        cmd42.Parameters.AddWithValue("@OfficeUseTabWFNO", objAccount.OfficeUseWF_No);
                        cmd42.Parameters.AddWithValue("@OfficeUseTabOfficerName", objAccount.OfficeUseNameAndSign);
                        cmd42.Parameters.AddWithValue("@OfficeUseTabTicketNo", objAccount.OfficeUseTicket_No);
                        cmd42.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        cmd42.Parameters.AddWithValue("@updateBy", "");

                        connection22.Open();
                        cmd42.ExecuteNonQuery();
                        connection22.Close();
                    }
                    return Json("Success");
                }
                else if (check == "SaveAOC")
                {
                    //objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_Account_AccountAOCDetails{custID},{objAccount.AOC_VISAorATM},{objAccount.AOC_SMSBanking},{objAccount.AOC_ACAndKYCComplied},{objAccount.AOC_IBComformationRequest},{objAccount.AOC_ChequeBookRequest},{objAccount.AOC_CosmoNetComformationRequest},{objAccount.AOC_AutoSweep},{objAccount.AOC_OfficerName},{objAccount.AOC_TicketNo}");
                    string conn = _connectionString;
                    using (SqlConnection connection32 = new SqlConnection(conn))
                    {
                        SqlCommand cmd72 = new SqlCommand("USP_Account_AccountAOCDetails", connection32);
                        cmd72.CommandType = CommandType.StoredProcedure;
                        cmd72.Parameters.AddWithValue("@CustomerID", custID);
                        cmd72.Parameters.AddWithValue("@AOCTabVISAORATM", objAccount.AOC_VISAorATM);
                        cmd72.Parameters.AddWithValue("@AOCTabSMS", objAccount.AOC_SMSBanking);
                        cmd72.Parameters.AddWithValue("@AOCTabACandKYC", objAccount.AOC_ACAndKYCComplied);
                        cmd72.Parameters.AddWithValue("@AOCTabIBComformationRequest", objAccount.AOC_IBComformationRequest);
                        cmd72.Parameters.AddWithValue("@AOCTabChequeBookRequest", objAccount.AOC_ChequeBookRequest);
                        cmd72.Parameters.AddWithValue("@AOCTabCosmoNetComformationRequest", objAccount.AOC_CosmoNetComformationRequest);
                        cmd72.Parameters.AddWithValue("@AOCTabAutoSweep", objAccount.AOC_AutoSweep);
                        cmd72.Parameters.AddWithValue("@AOCTabOfficerName", objAccount.AOC_OfficerName);
                        cmd72.Parameters.AddWithValue("@AOCTabTicketNo", objAccount.AOC_TicketNo);
                        connection32.Open();
                        cmd72.ExecuteNonQuery();
                        connection32.Close();
                    }
                    return Json("Success");
                }
                else if (check == "SaveABC")
                {
                    //objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_Account_AccountABCDetails{custID},{objAccount.ABCcell_OfficerName},{objAccount.ABCcell_TicketNo}");
                    string conn = _connectionString;
                    using (SqlConnection connection42 = new SqlConnection(conn))
                    {
                        SqlCommand cmd82 = new SqlCommand("USP_Account_AccountABCDetails", connection42);
                        cmd82.CommandType = CommandType.StoredProcedure;
                        cmd82.Parameters.AddWithValue("@CustomerID", custID);
                        cmd82.Parameters.AddWithValue("@ABCTabOfficerName", objAccount.ABCcell_OfficerName);
                        cmd82.Parameters.AddWithValue("@ABCTabTicketNo", objAccount.ABCcell_TicketNo);

                        connection42.Open();
                        cmd82.ExecuteNonQuery();
                        connection42.Close();
                    }
                    return Json("Success");
                }
                else if (check == "FormSave")
                {
                    //objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_Indo_SavingACCUpdateFlag{custID}");
                    string conn = _connectionString;

                    using (SqlConnection connection3 = new SqlConnection(conn))
                    {
                        SqlCommand cmd3 = new SqlCommand("USP_SavingACCFlag", connection3);
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.Parameters.AddWithValue("@CustId", custID);// Convert.ToInt64(HttpContext.Session.GetString("Ecid")));
                        connection3.Open();
                        SqlDataReader reader3 = cmd3.ExecuteReader();
                        if (reader3.Read())
                        {
                            //var Result = reader2["RESULT"].ToString();
                        }
                    }

                    return Json("Success");
                }
                else
                {
                    return View();
                }
                return Json("Success");
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }

        public ActionResult RulesAndRegulation()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                return PartialView("Views/CustomerAccount/RulesAndRegulation.cshtml");
                //return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }
        public bool IsMinor(DateTime dateOfBirth)
        {
            var age = CalculateAge(dateOfBirth);

            // check if age is less than 18
            if (age < 18)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Is18OrAbove(DateTime dateOfBirth)
        {
            var age = CalculateAge(dateOfBirth);

            // check if age is 18 or above
            if (age >= 18)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            // calculate age based on birthdate
            var age = DateTime.Today.Year - dateOfBirth.Year;

            // subtract a year if the birthdate hasn't occurred yet this year
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
        public ActionResult ViewRulesAndRegulation()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                ClsCustAccountFormDetails objPdf = new ClsCustAccountFormDetails();
                ClsSummeryDetails objPdf1 = new ClsSummeryDetails();


                var pdf = new RotativaCore.ActionAsPdf("RulesAndRegulation", objPdf)
                {
                    FileName = "indo.pdf",
                    PageSize = RotativaCore.Options.Size.Legal,
                    PageHeight = 750.00,
                    PageMargins = { Left = 5, Right = 5, Top = 0, Bottom = 0 }
                };
                byte[] storepdf = pdf.BuildPdf(ControllerContext);


                MemoryStream MS = new MemoryStream();
                MS.Write(storepdf, 0, storepdf.Length);
                MS.Position = 0;
                return File(MS, "application/pdf");

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }


        public ActionResult DigitalAccKYCPdf()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                long CustomerId = 10;// Convert.ToInt64(Session["PersonalId"]);
                ClsSummeryDetails objSummery = new ClsSummeryDetails();

                objSummery.AccOpenDate = DateTime.Now.ToString();
                objSummery.Branch = "Cosmos";
                objSummery.BranchSolID = "1";
                var result = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerDetails {CustomerId},{""}").AsEnumerable().FirstOrDefault();
                var CustId = CustomerId;
                objSummery.CustomerId = Convert.ToString(CustomerId);
                objSummery.CFirstName = result.FirstName;
                objSummery.CMiddleName = result.MiddleName;
                objSummery.CLastName = result.LastName;
                objSummery.CGender = result.Gender;
                objSummery.CMobileNo = result.MobileNo;
                objSummery.CEmailId = result.EmailId;
                objSummery.ManualPanNo = result.PanNo;
                objSummery.Pin_Code = result.PinCode;
                objSummery.DOB = result.Dob;
                if (objSummery.DOB.Contains('-'))
                {
                    string[] DateOfBirth = objSummery.DOB.Split('-');
                    objSummery.DOB_MM = DateOfBirth[0];
                    objSummery.DOB_DD = DateOfBirth[1];
                    string[] year = DateOfBirth[2].Split(' ');
                    objSummery.DOB_yyyy = year[0];
                }
                else if (objSummery.DOB.Contains('/'))
                {
                    string[] DateOfBirth = objSummery.DOB.Split('/');
                    objSummery.DOB_MM = DateOfBirth[0];
                    objSummery.DOB_DD = DateOfBirth[1];
                    string[] year = DateOfBirth[2].Split(' ');
                    objSummery.DOB_yyyy = year[0];
                }
                if (result.LivePhoto != null)
                {
                    objSummery.LivePhoto = Convert.ToBase64String(result.LivePhoto);
                }
                objSummery.CAddress = result.ClientAddress1;
                return View(objSummery);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }
        public ActionResult DownloadAccpdf()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var aq = Convert.ToInt64(HttpContext.Session.GetString("DAEditCustomerdetailId"));
                if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
                {
                    var PersonalId = (HttpContext.Session.GetString("DAEditCustomerdetailId"));
                    var qscq = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                    ViewBag.AdminFlag = "AdminFlag";
                }
                ClsSummeryDetails objPdf = new ClsSummeryDetails();
                var qsc = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                long CustomerId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                var Accresult = objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_Account_GetAccountDetails {CustomerId}").AsEnumerable().FirstOrDefault();

                if (Accresult.AccountType != null || Accresult.AccountType != "")
                {

                    if (Accresult.AccountType == "CurrentAccount")
                    {
                        objPdf.Regular = true;
                    }
                    if (Accresult.AccountType == "SavingAccount")
                    {
                        objPdf.CosmoPremium = true;
                    }
                    if (Accresult.AccountType == "CosmoSalary")
                    {
                        objPdf.CosmoSalary = true;
                    }
                    if (Accresult.AccountType == "CosmoRoyale")
                    {
                        objPdf.CosmoRoyale = true;
                    }
                    if (Accresult.AccountType == "CosmoPremiumPlus")
                    {
                        objPdf.CosmoPremiumPlus = true;
                    }
                    if (Accresult.AccountType == "BSBDA")
                    {
                        objPdf.BSBDA = true;
                    }
                    if (Accresult.AccountType == "CosmoYouth")
                    {
                        objPdf.CosmoYouth = true;
                    }
                    if (Accresult.AccountType == "CosmoPremiumSalary")
                    {
                        objPdf.CosmoPremiumSalary = true;
                    }
                    if (Accresult.AccountType == "other")
                    {
                        objPdf.other = true;
                    }
                }
                objPdf.AccOpenDate = DateTime.Now.ToString();
                //objPdf.Branch = Accresult.Branch;
                string conn1 = _connectionString;
                using (SqlConnection connection12 = new SqlConnection(conn1))
                {
                    SqlCommand cmd12 = new SqlCommand("USP_GetBranchName", connection12);
                    cmd12.CommandType = CommandType.StoredProcedure;

                    cmd12.Parameters.AddWithValue("@Branch_Code", Accresult.Branch);
                    connection12.Open();
                    SqlDataReader reader1 = cmd12.ExecuteReader();
                    if (reader1.Read())
                    {


                        var Branch = reader1[2].ToString();
                        objPdf.Branch = Branch;
                    }
                }
                objPdf.ckycIdentifier = Accresult.CkycNumber;
                objPdf.MaidenPrefixName = Accresult.MaidenPrefix;
                objPdf.MaidenFName = Accresult.MaidenFname;
                objPdf.MaidenMName = Accresult.MaidenMname;
                objPdf.MaidenLName = Accresult.MaidenLname;
                objPdf.FatherPrefixName = Accresult.FatherSpousePrefix;
                objPdf.FatherFName = Accresult.FatherSpouseFname;
                objPdf.FatherMName = Accresult.FatherSpouseMname;
                objPdf.FatherLName = Accresult.FatherSpouseLname;
                objPdf.MotherPrefixName = Accresult.MotherPrefix;
                objPdf.MotherFName = Accresult.MotherFname;
                objPdf.MotherMName = Accresult.MotherMname;
                objPdf.MotherLName = Accresult.MotherLname;
                objPdf.BirthPlaceCity = Accresult.BirthPlaceCity;
                objPdf.BirthPlaceCountry = Accresult.BirthPlaceCountry;
                objPdf.MaritalStatus = Accresult.MaritalStatus;
                objPdf.Religion = Accresult.Religion;
                objPdf.Caste = Accresult.Caste;
                objPdf.OccupationType = Accresult.OccupationType;
                objPdf.BusinessFirm = Accresult.BusinessFirm;
                objPdf.SalaryEmployed = Accresult.SalariedEmployed;
                objPdf.Designation = Accresult.Designation;
                objPdf.AnnualIncome = Accresult.AnnualIncome;
                objPdf.EducationQualification = Accresult.EducationQualification;
                objPdf.ThresholdLimit = Accresult.ThresholdLimit;
                objPdf.NatureOfOrganization = Accresult.NatureOrganisation;
                objPdf.NatureOfOrganizationRemark = Accresult.NatureOrganisationOtherRemark;
                objPdf.initialDepositAmount = Accresult.InitialDepositAmount;
                objPdf.initialDepositChequeDate = Accresult.InitialDepositDate.ToString();
                objPdf.Nationality = Accresult.Nationality;
                objPdf.initialDepositAmount = Accresult.InitialDepositAmount;
                objPdf.initialDepositChequeNo = Accresult.InitialDepositChequeNo;
                objPdf.initialDepositChequeDate = Accresult.InitialDepositDate.ToString();
                objPdf.CarLoan = Convert.ToBoolean(Accresult.CarLoan);
                objPdf.ConsumerLoan = Convert.ToBoolean(Accresult.ConsumerLoan);
                objPdf.HomeLoan = Convert.ToBoolean(Accresult.HomeLoan);
                objPdf.BusinessLoan = Convert.ToBoolean(Accresult.BusinessLoan);
                objPdf.EducationLoan = Convert.ToBoolean(Accresult.EducationLoan);
                objPdf.NewsPaper = Convert.ToBoolean(Accresult.Newspaper);
                objPdf.Staff = Convert.ToBoolean(Accresult.Staff);
                objPdf.RelativeFriend = Convert.ToBoolean(Accresult.RelativeOrfriend);
                objPdf.OtherCreditFacility = Convert.ToBoolean(Accresult.OtherCredit);
                objPdf.CosmoRupayCard = Convert.ToBoolean(Accresult.EbankCosmoRupay);
                objPdf.CosmoVisaDebitCard = Convert.ToBoolean(Accresult.EbankCosmoVisa);
                objPdf.UPI = Convert.ToBoolean(Accresult.EUPI);
                objPdf.InternetBanking = Convert.ToBoolean(Accresult.EInternetBanking);
                objPdf.IMPS = Convert.ToBoolean(Accresult.EIMPS);

                //objPdf.Branch = Accresult.Branch;
                objPdf.MaritalStatus = Accresult.MaritalStatus;
                objPdf.ResidentialStatus = Accresult.ResidentialStatus;
                objPdf.OccupationType = Accresult.OccupationType;
                objPdf.AnnualIncome = Accresult.AnnualIncome;
                objPdf.NatureOfOrganization = Accresult.NatureOrganisation;
                objPdf.NatureOfOrganizationRemark = Accresult.NatureOrganisationOtherRemark;
                objPdf.LivePhoto = (string.Format("data:image/jpg;base64,{0}", HttpContext.Session.GetString("liveimage")));
                if (Accresult.NominationYesOrNo == true)
                {
                    objPdf.NomineeForAccountYes = true;
                }
                if (Accresult.NominationYesOrNo == false)
                {
                    objPdf.NomineeForAccountNo = false;
                }
                objPdf.NomineePrefix = Accresult.NominationPreName;
                objPdf.NomineeFName = Accresult.NominationFname;
                objPdf.NomineeMName = Accresult.NominationMname;
                objPdf.NomineeLName = Accresult.NominationLname;
                objPdf.Nominee_ADDRESS_1 = Accresult.NominationAddress1;
                objPdf.Nominee_ADDRESS_2 = Accresult.NominationAddress2;
                objPdf.Nominee_ADDRESS_3 = Accresult.NominationAddress3;
                objPdf.NomineeAge = Convert.ToString(Accresult.NomineeAge);
                objPdf.NomineeRelation = Accresult.NomineeRelation;
                objPdf.Nominee_CITY = Accresult.Nominationcity;
                //objPdf.Nominee_COUNTRY = Accresult.NominationCountry;
                
                using (SqlConnection connection12 = new SqlConnection(conn1))
                {
                    SqlCommand cmd12 = new SqlCommand("USP_GetCountryId", connection12);
                    cmd12.CommandType = CommandType.StoredProcedure;

                    cmd12.Parameters.AddWithValue("@CountryCode", Accresult.NominationCountry);
                    connection12.Open();
                    SqlDataReader reader1 = cmd12.ExecuteReader();
                    if (reader1.Read())
                    {


                        var Country = reader1[2].ToString();
                        objPdf.Nominee_COUNTRY = Country;
                    }
                }
                //objPdf.Nominee_STATE = Accresult.NominationState;
                using (SqlConnection connection12 = new SqlConnection(conn1))
                {
                    SqlCommand cmd12 = new SqlCommand("USP_GetStateName", connection12);
                    cmd12.CommandType = CommandType.StoredProcedure;

                    cmd12.Parameters.AddWithValue("@State_Code", Accresult.NominationState);
                    connection12.Open();
                    SqlDataReader reader1 = cmd12.ExecuteReader();
                    if (reader1.Read())
                    {


                        var state = reader1[2].ToString();
                        objPdf.Nominee_STATE = state;
                    }
                }
                objPdf.Nominee_Pincode = Accresult.NominationPincode;
                if (Accresult.IsJointHolder == true)
                {
                    objPdf.JointForAccountYes = true;
                }
                if (Accresult.IsJointHolder == false)
                {
                    objPdf.JointForAccountNo = false;
                }

                var result = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerDetails {CustomerId}").AsEnumerable().FirstOrDefault();


                string[] DateOfBirth = null;
                string[] year = null;
                objPdf.CFirstName = result.FirstName;
                objPdf.CMiddleName = result.MiddleName;
                objPdf.CLastName = result.LastName;
                objPdf.CGender = result.Gender;
                objPdf.CEmailId = result.EmailId;
                objPdf.CustomerId = Convert.ToString(result.CustomerDetailId);
                objPdf.CMobileNo = result.MobileNo;
                objPdf.DOB = result.Dob;
                objPdf.ManualPanNo = ObjTripleDes.Decrypt(result.PanNo);
                objPdf.Pin_Code = result.PinCode;
                objPdf.Referencenumber = result.ReferenceNumber;

                if (objPdf.DOB.Contains('-'))
                {
                    DateOfBirth = objPdf.DOB.Split('-');
                    objPdf.DOB_MM = DateOfBirth[1];
                    objPdf.DOB_DD = DateOfBirth[0];
                    year = DateOfBirth[2].Split(' ');
                    objPdf.DOB_yyyy = year[0];
                    DateTime date = DateTime.ParseExact(result.Dob, "dd-MM-yyyy", null);
                    objPdf.CDateOfBirth = Convert.ToDateTime(date);
                }
                else if (objPdf.DOB.Contains('/'))
                {
                    DateOfBirth = objPdf.DOB.Split('/');
                    objPdf.DOB_MM = DateOfBirth[0];
                    objPdf.DOB_DD = DateOfBirth[1];
                    year = DateOfBirth[2].Split(' ');
                    objPdf.DOB_yyyy = year[2];
                    DateTime date = DateTime.ParseExact(result.Dob, "dd/MM/yyyy", null);
                    objPdf.CDateOfBirth = Convert.ToDateTime(date);
                }

                if (result.Photo != null)
                {
                    objPdf.AadharPhoto = Convert.ToBase64String(result.Photo);
                }
                if (result.LivePhoto != null)
                {
                    objPdf.LivePhoto = Convert.ToBase64String(result.LivePhoto);
                }
                objPdf.CAddress = result.ClientAddress1;

                var objDocResult = objDetails.AdmCustomerDocumentsDetails1.FromSqlRaw($"USP_GetDocumentById {CustomerId}").ToList();

                foreach (var listDoc in objDocResult)
                {
                    switch (listDoc.DocMainCategory)
                    {
                        case "P":
                            ViewBag.Cust_Photo = listDoc.CustomerDocumentId;
                            dochistory_Photo = listDoc.DocumentHistory;
                            var base64Photo = Convert.ToBase64String(listDoc.DocumentHistory);
                            imgtypePhoto = listDoc.DocumentName.Split('.').LastOrDefault();
                            objPdf.imgtypePhoto = imgtypePhoto;
                            objPdf.PhotoDocument = String.Format("data:image/" + imgtypePhoto + ";base64,{0}", base64Photo);
                            break;
                        case "I":
                            dochistory_POI = listDoc.DocumentHistory;
                            ViewBag.Cust_POI = listDoc.CustomerDocumentId;
                            var base64_POI = Convert.ToBase64String(dochistory_POI);
                            imgtypePOI = listDoc.DocumentName.Split('.').LastOrDefault();
                            if (imgtypePOI == "pdf")
                            {
                                objPdf.imgtypePOI = imgtypePOI;
                                objPdf.POI_Document = String.Format("data:image/" + imgtype_POI + ";base64,{0}", base64_POI);
                            }
                            else if (imgtypePOI == "jpg" || imgtypePOI == "JPEG" || imgtypePOI == "Jpg" || imgtypePOI == "jpeg")
                            {
                                objPdf.imgtypePOI = imgtypePOI;
                                objPdf.POI_Document = String.Format("data:image/" + imgtype_POI + ";base64,{0}", base64_POI);
                            }
                            else
                            {
                                objPdf.imgtypePOI = imgtypePOI;
                                objPdf.POI_Document = String.Format("data:image/" + imgtype_POI + ";base64,{0}", base64_POI);
                            }

                            break;
                        case "CA":
                            dochistory_CA = listDoc.DocumentHistory;
                            var base64_CA = Convert.ToBase64String(listDoc.DocumentHistory);
                            imgtype_CA = listDoc.DocumentName.Split('.').LastOrDefault();
                            if (imgtype_CA == "pdf")
                            {
                                objPdf.imgtypeCA = imgtype_CA;
                                objPdf.CA_Document = String.Format("data:image/" + imgtype_CA + ";base64,{0}", base64_CA);

                            }
                            else if (imgtype_CA == "jpg" || imgtype_CA == "JPEG" || imgtype_CA == "Jpg" || imgtype_CA == "jpeg")
                            {
                                objPdf.imgtypeCA = imgtype_CA;
                                objPdf.CA_Document = String.Format("data:image/" + imgtype_CA + ";base64,{0}", base64_CA);

                            }
                            else if (imgtype_CA == "POACam")
                            {
                                objPdf.imgtypeCA = imgtype_CA;
                                objPdf.CA_Document = String.Format("data:image/" + imgtype_CA + ";base64,{0}", base64_CA);

                            }
                            else
                            {

                            }

                            break;
                        case "DL":
                            dochistory_POI = listDoc.DocumentHistory;
                            ViewBag.Cust_POI = listDoc.CustomerDocumentId;
                            var base64_POI1 = Convert.ToBase64String(dochistory_POI);
                            imgtypePOI = listDoc.DocumentName.Split('.').LastOrDefault();
                            objPdf.imgtypePOI = imgtypePOI;
                            objPdf.POI_Document1 = String.Format("data:image/" + imgtype_POI + ";base64,{0}", base64_POI1);

                            break;
                        case "SI":
                            dochistory_SI = listDoc.DocumentHistory;
                            ViewBag.Cust_SI = listDoc.CustomerDocumentId;
                            var base64_SI = Convert.ToBase64String(dochistory_SI);
                            imgtypeSI = listDoc.DocumentName.Split('.').LastOrDefault();
                            objPdf.imgtypeSI = imgtypeSI;
                            objPdf.SI_Document = String.Format("data:image/" + imgtypeSI + ";base64,{0}", base64_SI);
                            break;

                    }
                }

                return new Rotativa.AspNetCore.ViewAsPdf("DownloadAccpdf", objPdf)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                };
            }

            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        public ActionResult OfficeUsePdf()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                ClsSummeryDetails objSummery = new ClsSummeryDetails();


                return View(objSummery);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        public static byte[] Append(byte[] inputPdf, params System.Drawing.Image[] images)
        {
            using (var ms = new MemoryStream())
            {
                var pdf = new PdfReader(inputPdf);
                var doc = new iTextSharp.text.Document(pdf.GetPageSizeWithRotation(1));
                using (var writer = PdfWriter.GetInstance(doc, ms))
                {
                    doc.Open();
                    for (int page = 0; page < pdf.NumberOfPages; page++)
                    {
                        doc.SetPageSize(pdf.GetPageSizeWithRotation(page + 1));
                        doc.NewPage();
                        var pg = writer.GetImportedPage(pdf, page + 1);
                        int rotation = pdf.GetPageRotation(page + 1);
                        if (rotation == 90 || rotation == 270)
                            writer.DirectContent.AddTemplate(
                                pg, 0, -1f, 1f, 0, 0, pdf.GetPageSizeWithRotation(page).Height);
                        else
                            writer.DirectContent.AddTemplate(pg, 1f, 0, 0, 1f, 0, 0);
                    }
                    foreach (var image in images)
                    {
                        doc.NewPage();

                        ImageFormat format = image.PixelFormat == PixelFormat.Format1bppIndexed
                                             || image.PixelFormat == PixelFormat.Format4bppIndexed
                                             || image.PixelFormat == PixelFormat.Format8bppIndexed
                                                 ? ImageFormat.Tiff
                                                 : ImageFormat.Jpeg;
                        var pdfImage = iTextSharp.text.Image.GetInstance(image, format);
                        pdfImage.Alignment = Element.ALIGN_CENTER;
                        pdfImage.ScaleToFit(doc.PageSize.Width - 10, doc.PageSize.Height - 10);

                        doc.Add(pdfImage);
                    }
                    doc.Close();
                    writer.Close();
                }
                ms.Flush();
                return ms.GetBuffer();
            }
        }


        public byte[] removePagesFromPdf(byte[] sourceFile, int TotalPage)
        {
            PdfReader r = new PdfReader(sourceFile);
            byte[] fsbyte;
            using (MemoryStream fs = new MemoryStream())
            {
                using (iTextSharp.text.Document doc = new iTextSharp.text.Document())
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, fs))
                    {
                        doc.Open();
                        for (int page = 2; page <= TotalPage; page++)
                        {
                            doc.NewPage();
                            w.DirectContent.AddTemplate(w.GetImportedPage(r, page), 0, 0);
                        }
                        doc.Close();
                        fsbyte = fs.GetBuffer();
                    }
                }
            }
            return fsbyte;
        }

        public static byte[] mergePdf(byte[] pdf1, byte[] pdf2)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                MemoryStream outStream = new MemoryStream();
                using (iTextSharp.text.Document document = new iTextSharp.text.Document())
                using (PdfCopy copy = new PdfCopy(document, outStream))
                {
                    document.Open();
                    PdfReader.unethicalreading = true;

                    copy.AddDocument(new PdfReader(pdf1));
                    copy.AddDocument(new PdfReader(pdf2));
                    document.Close();
                }
                return outStream.ToArray();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return null;
            }
        }

        public static void SendMailToCustomer(string mailBody, string to, string CustomerId)
        {
            ErrorLog error_log = new ErrorLog();
            string from = "amruta.alphafinsoft@outlook.com"; //From address    
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Application Form";
            message.Body = mailBody;
            string src = System.Configuration.ConfigurationManager.AppSettings["FinalPDF"];
            FileInfo file = new FileInfo(src + Path.DirectorySeparatorChar + CustomerId + "" + ".Pdf"); //"AQZPJ2551Q"
            if (file.Exists)
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(src + Path.DirectorySeparatorChar + CustomerId + "" + ".Pdf");
                message.Attachments.Add(attachment);
            }
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.live.com", 587); //Outlook smtp    
            client.Credentials = new System.Net.NetworkCredential()
            {
                UserName = from,
                Password = "Maugiri@1"
            };
            client.EnableSsl = true;
            try
            {
                Console.WriteLine("Attempting to send email...");
                client.Send(message);
                Console.WriteLine("Email sent!");
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                Console.WriteLine("The email was not sent.");
                Console.WriteLine("Error message: " + ex.Message);
            }
        }

        public string custData(string IDtype, string IDnumber)
        {
            var encreptPan = ObjTripleDes.Encrypt(IDnumber);
            //var custresult = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_Account_GetDetailsOnIDtype {IDtype},{encreptPan}").AsEnumerable().FirstOrDefault();

            var custresult = objDetails.AdmKycCustomerDetails.FromSqlRaw("USP_Account_GetDetailsOnIDtype @IdType, @IdNumber", new SqlParameter("@IdType", IDtype), new SqlParameter("@IdNumber", encreptPan)).AsEnumerable().FirstOrDefault();


            if (custresult != null)
            {
                HttpContext.Session.SetString("CustJoint", (Convert.ToString(custresult.CustomerDetailId)));
                return custresult.FirstName + "#" + custresult.MiddleName + "#" + custresult.LastName + "#" + custresult.CustomerDetailId;
            }
            else
            {
                return "Not Found";
            }
        }
        public string NomineecustData(string IDtype, string IDnumber)
        {
            var IDnumberEncrypted = ObjTripleDes.Encrypt(IDnumber);
            //var custresult = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_Account_GetDetailsOnIDtype {IDtype},{IDnumberEncrypted}").AsEnumerable().FirstOrDefault();
            string conn12 = _connectionString;
            using (SqlConnection connection1 = new SqlConnection(conn12))
            {
                SqlCommand cmd17 = new SqlCommand("USP_Account_GetDetailsOnIDtype", connection1);
                cmd17.CommandType = CommandType.StoredProcedure;
                cmd17.Parameters.AddWithValue("@IdType", IDtype);
                cmd17.Parameters.AddWithValue("@IdNumber", IDnumberEncrypted);
                connection1.Open();
                SqlDataReader reader1 = cmd17.ExecuteReader();
                reader1.Read();
                HttpContext.Session.SetString("CustJoint", (Convert.ToString(reader1["CustomerDetailId"])));

                if (reader1 != null)
                {
                    return reader1["FirstName"] + "#" + reader1["MiddleName"] + "#" + reader1["LastName"] + "#" + reader1["CustomerDetailId"];

                }
                else
                {
                    return "Not Found";
                }
            }
        }
        public ActionResult BSBDADeclaration()
        {
            return View();
        }

        public ActionResult CosmoPremiumorRoyaleDeclaration()
        {
            return View();

        }

        public ActionResult SelfDeclaration()
        {
            return View();
        }
        public IActionResult CBSApproveDate()
        {
            var CustomerID = HttpContext.Session.GetString("DAEditCustomerdetailId");

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("USP_AppDateSil", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustId", CustomerID);


                cmd.ExecuteNonQuery();
            }
            return Json("");
        }
        public ActionResult CBSCustomerIDCreation()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var CustomerID = HttpContext.Session.GetString("DAEditCustomerdetailId");
                ClsJointAccountFinacle joint = new ClsJointAccountFinacle();
                ClsSavingAccountFinacle objroot = new ClsSavingAccountFinacle();

                byte[] firstjointsignature = null;
                byte[] secondjointsignature = null;
                string firstjointsignature1 = "";
                string secondjointsignature1 = "";

                var Result0 = objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_GetJointAccountDetails {CustomerID} ").AsEnumerable().FirstOrDefault();
                if (Result0.IsJointHolder == true)
                {

                    int JointHolderCount = Result0.JointHolderCount;
                    for (int i = 1; i <= JointHolderCount; i++)
                    {

                        if (i == 1)
                        {
                            var Result1 = objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_GetJointHolderDetails {CustomerID},{i}").AsEnumerable().FirstOrDefault();
                            var Result5 = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetDetailsForJoint {Result1.CustomerId}").AsEnumerable().FirstOrDefault();
                            var resultforsignature = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetCustomerSignature {Result1.CustomerId}").AsEnumerable().FirstOrDefault();

                            objAccount.firstapplicantFNAME = Result1.CustomerFname;
                            objAccount.firstapplicantMNAME = Result1.CustomerMname;
                            objAccount.firstapplicantLNAME = Result1.CustomerLname;
                            objAccount.AnnualIncome1 = Result1.AnnualIncome;
                            objAccount.Branch1 = Result1.Branch;
                            objAccount.Caste1 = Result1.Caste;
                            objAccount.ckycIdentifier1 = Result1.CkycNumber;
                            objAccount.MaidenPrefixName1 = Result1.MaidenPrefix;
                            objAccount.MaidenFName1 = Result1.MaidenFname;
                            objAccount.MaidenMName1 = Result1.MaidenMname;
                            objAccount.MaidenPrefixName1 = Result1.MaidenPrefix;
                            objAccount.MaidenLName1 = Result1.MaidenLname;
                            objAccount.MaritalStatus1 = Result5.maritalstatus;
                            objAccount.MotherFName1 = Result1.MotherFname;
                            objAccount.MotherMName1 = Result1.MotherMname;
                            objAccount.MotherLName1 = Result1.MotherLname;
                            objAccount.Nationality1 = Result1.Nationality;
                            objAccount.OccupationType1 = Result1.OccupationType;
                            objAccount.Religion1 = Result5.Religion;
                            objAccount.ResidentialStatus1 = Result1.ResidentialStatus;
                            objAccount.ThresholdLimit1 = Result1.ThresholdLimit;
                            objAccount.NatureOfOrganization1 = Result1.NatureOrganisation;
                            objAccount.FJointAdd1 = Result5.ClientPermAddress1;
                            objAccount.FJointAdd2 = Result5.ClientPermAddress2;
                            objAccount.FJointAdd3 = Result5.ClientPermAddress3;
                            objAccount.FCity = Result5.ClientPermCity;
                            objAccount.FPin = Result5.ClientPermPin;
                            objAccount.FState = Result5.ClientPermState;
                            objAccount.FCountry = Result5.CountryId;
                            objAccount.FDob = Result5.Dob;
                            objAccount.FEmailId = Result5.EmailId;
                            objAccount.FMobileNo = Result5.MobileNo;
                            objAccount.FPanNo = Result5.PanNo;
                            objAccount.FGender = Result5.Gender;
                            objAccount.FResidence = Result5.Residence;
                            objAccount.FDocument = Result5.ResidenceDocument;
                            objAccount.FPassportNo = Result5.PassportNo;
                            objAccount.FResidenceYN = Result5.residenceYN;
                            objAccount.FPhoneBAnking = Result5.PhoneBanking;
                            objAccount.FSubTitle = Result5.SubTitle;
                            firstjointsignature = resultforsignature.DocumentHistory;

                        }
                        if (i == 2)
                        {
                            var Result2 = objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_GetJointHolderDetails {CustomerID},{i}").AsEnumerable().FirstOrDefault();
                            var Result6 = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetDetailsForJoint {Result2.CustomerId}").AsEnumerable().FirstOrDefault();
                            var resultforsignature2 = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetCustomerSignature {Result2.CustomerId}").AsEnumerable().FirstOrDefault();

                            objAccount.SecondapplicantFNAME = Result2.CustomerFname;
                            objAccount.SecondapplicantMNAME = Result2.CustomerMname;
                            objAccount.SecondapplicantLNAME = Result2.CustomerLname;
                            objAccount.AnnualIncome2 = Result2.AnnualIncome;
                            objAccount.Branch2 = Result2.Branch;
                            objAccount.Caste2 = Result2.Caste;
                            objAccount.ckycIdentifier2 = Result2.CkycNumber;
                            objAccount.MaidenPrefixName2 = Result2.MaidenPrefix;
                            objAccount.MaidenFName2 = Result2.MaidenFname;
                            objAccount.MaidenMName2 = Result2.MaidenMname;
                            objAccount.MaidenPrefixName2 = Result2.MaidenPrefix;
                            objAccount.MaidenLName2 = Result2.MaidenLname;
                            objAccount.MaritalStatus2 = Result6.maritalstatus;
                            objAccount.MotherFName2 = Result2.MotherFname;
                            objAccount.MotherMName2 = Result2.MotherMname;
                            objAccount.MotherLName2 = Result2.MotherLname;
                            objAccount.Nationality2 = Result2.Nationality;
                            objAccount.OccupationType2 = Result2.OccupationType;
                            objAccount.Religion2 = Result6.Religion;
                            objAccount.ResidentialStatus2 = Result2.ResidentialStatus;
                            objAccount.ThresholdLimit2 = Result2.ThresholdLimit;
                            objAccount.NatureOfOrganization2 = Result2.NatureOrganisation;
                            objAccount.SJointAdd1 = Result6.ClientPermAddress1;
                            objAccount.SJointAdd2 = Result6.ClientPermAddress2;
                            objAccount.SJointAdd3 = Result6.ClientPermAddress3;
                            objAccount.SCity = Result6.ClientPermCity;
                            objAccount.SPin = Result6.ClientPermPin;
                            objAccount.SState = Result6.ClientPermState;
                            objAccount.SCountry = Result6.CountryId;
                            objAccount.SDob = Result6.Dob;
                            objAccount.SEmailId = Result6.EmailId;
                            objAccount.SMobileNo = Result6.MobileNo;
                            objAccount.SPanNo = Result6.PanNo;
                            objAccount.SGender = Result6.Gender;
                            objAccount.SResidence = Result6.Residence;
                            objAccount.SDocument = Result6.ResidenceDocument;
                            objAccount.SPassportNo = Result6.PassportNo;
                            objAccount.SResidenceYN = Result6.residenceYN;
                            objAccount.SPhoneBAnking = Result6.PhoneBanking;
                            objAccount.SSubTitle = Result6.SubTitle;
                            secondjointsignature = resultforsignature2.DocumentHistory;

                        }
                        if (i == 3)
                        {
                            var Result3 = objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_GetJointHolderDetails {CustomerID},{i}").AsEnumerable().FirstOrDefault();
                            var Result7 = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetDetailsForJoint {Result3.CustomerId}").AsEnumerable().FirstOrDefault();

                            objAccount.ThirdapplicantFNAME = Result3.CustomerFname;
                            objAccount.ThirdapplicantMNAME = Result3.CustomerMname;
                            objAccount.ThirdapplicantLNAME = Result3.CustomerLname;
                            objAccount.AnnualIncome3 = Result3.AnnualIncome;
                            objAccount.Branch3 = Result3.Branch;
                            objAccount.Caste3 = Result3.Caste;
                            objAccount.ckycIdentifier3 = Result3.CkycNumber;
                            objAccount.MaidenPrefixName3 = Result3.MaidenPrefix;
                            objAccount.MaidenFName3 = Result3.MaidenFname;
                            objAccount.MaidenMName3 = Result3.MaidenMname;
                            objAccount.MaidenPrefixName3 = Result3.MaidenPrefix;
                            objAccount.MaidenLName3 = Result3.MaidenLname;
                            objAccount.MaritalStatus3 = Result7.maritalstatus;
                            objAccount.MotherFName3 = Result3.MotherFname;
                            objAccount.MotherMName3 = Result3.MotherMname;
                            objAccount.MotherLName3 = Result3.MotherLname;
                            objAccount.Nationality3 = Result3.Nationality;
                            objAccount.OccupationType3 = Result3.OccupationType;
                            objAccount.Religion3 = Result7.Religion;
                            objAccount.ResidentialStatus3 = Result3.ResidentialStatus;
                            objAccount.ThresholdLimit3 = Result3.ThresholdLimit;
                            objAccount.NatureOfOrganization3 = Result3.NatureOrganisation;
                            objAccount.TJointAdd1 = Result7.ClientPermAddress1;
                            objAccount.TJointAdd2 = Result7.ClientPermAddress2;
                            objAccount.TJointAdd3 = Result7.ClientPermAddress3;
                            objAccount.TCity = Result7.ClientPermCity;
                            objAccount.TPin = Result7.ClientPermPin;
                            objAccount.TState = Result7.ClientPermState;
                            objAccount.TCountry = Result7.CountryId;
                            objAccount.TDob = Result7.Dob;
                            objAccount.TEmailId = Result7.EmailId;
                            objAccount.TMobileNo = Result7.MobileNo;
                            objAccount.TPanNo = Result7.PanNo;
                            objAccount.TGender = Result7.Gender;
                            objAccount.TResidence = Result7.Residence;
                            objAccount.TDocument = Result7.ResidenceDocument;
                            objAccount.TPassportNo = Result7.PassportNo;
                            objAccount.TResidenceYN = Result7.residenceYN;
                            objAccount.TPhoneBAnking = Result7.PhoneBanking;
                            objAccount.TSubTitle = Result7.SubTitle;
                        }
                    }
                }

                var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                string conn = _connectionString;
                using (SqlConnection connection11 = new SqlConnection(conn))
                {
                    SqlCommand cmd11 = new SqlCommand("USP_SavingAccountDetails", connection11);
                    cmd11.CommandType = CommandType.StoredProcedure;

                    cmd11.Parameters.AddWithValue("@CustomerId", CustomerId);
                    connection11.Open();
                    SqlDataReader reader = cmd11.ExecuteReader();
                    if (reader.Read())
                    {
                        var resul1 = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetCustomerSignature {CustomerId}").AsEnumerable().FirstOrDefault();
                        var response1 = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_SavingAccountDetails {CustomerId}").AsEnumerable().FirstOrDefault(); //reader["result"].ToString();

                        var response2 = objDetails.AdmCosmosCustomerDetails.FromSqlRaw($"USP_GetCustDetails {CustomerId}").AsEnumerable().FirstOrDefault();
                        string title = response1.SubTitle;//"MR";
                        string longname = response1.FirstName.ToUpper() + " " + response1.MiddleName.ToUpper() + " " + response1.LastName.ToUpper();// "Anand raj";
                        string FatherName = response1.MiddleName.ToUpper();//"Moorthy";
                        string Add1 = response1.ClientPermAddress1.ToUpper();//"House 01";
                        string Add2 = response1.ClientPermAddress2.ToUpper();//"Bldg 2";
                        string Add3 = response1.ClientPermAddress3.ToUpper();//"Sai Nagar";
                        string Area = response1.ClientPermCity.ToUpper(); //"001";
                        string City = response1.ClientPermCity.ToUpper();// "";
                        string PinCode = response1.ClientPermPin;//"25";
                        string State = response1.ClientPermState;//"MAH";
                        string Country = "IN"; //response1.CountryId;//
                        string Nationality = "Indian";
                        string Mobile = response1.MobileNo;//"9595954805";
                        string EmailID = response1.EmailId;//"apb@gmail.com";
                        string Introducer = "N";
                        string IntroducerCustomerNumber = "";
                        string PanNo = ObjTripleDes.Decrypt(response1.PanNo);//"BJJPMO259C";
                        string PanDesc = "";
                        string TDSYN = "";
                        string TDSReasonCd = "";
                        string Form15App = "";
                        string Date = DateTime.Now.ToString("dd-MM-yyyy");//"dd-MM-yyyy";
                        string DOB = response1.Dob;//"17-04-1992";
                        string SexCode = response1.Gender;//"F";
                        string CasteCd = response1.CasteCd;//"1";
                        string Religion = response1.Religion;// "000000001";
                        string MaritalStatus = response1.maritalstatus;//"000000001";
                        string MarriageDate = "000000003";
                        string Noofchildren = "";
                        string Bloodgroup = "";
                        string Residence = response1.Residence;//"000000003";
                        string document = response1.ResidenceDocument;//"000000001";
                        string CreditCardNumber = "";
                        string DateValid = "";
                        string ResidenceYN = response1.residenceYN;//"Y"; //0000001";
                        string PassportNumber = response1.PassportNo;
                        string ResidenceStatus = "000000001";//response1.ResidentialStatus;//"000000001";
                        string VisaValidUpto = "";
                        string AccountType = "SB";//response2.AccountType;// "SB";
                        string PhoneBankingYN = response1.PhoneBanking;//"N";
                        string test = "";
                        string AmlRating = response1.AMLRating;
                        string PhysicalChaYN = response2.PhysicallyChall_YN;
                        string PhysicalChalDesc = response2.PhysicallyChall;
                        string VerifiedDoc = response2.VerifyDOC;
                        string AnnualIncome = response2.AnnualIncome;
                        string Occupation = response2.OccupationType;
                        string Qualification = response2.EducationQualification;
                        string NomName = response2.NominationFname.ToUpper() + " " + response2.NominationLname.ToUpper();
                        string Relationship = response2.NomineeRelation;
                        string NomAdd1 = response2.NominationAddress1.ToUpper();
                        string NomAdd2 = response2.NominationAddress2.ToUpper();
                        string NomAdd3 = response2.NominationAddress3.ToUpper();
                        string NomAdd4 = "";
                        DateTime birthDate =response2.NomineeAge;
                        DateTime today = DateTime.Today;
                        int age = today.Year - birthDate.Year;
                        string Age =Convert.ToString( age);
                        string MinorYN;
                        if (age<18)
                        {
                            MinorYN = "Y";
                        }
                        else
                        {
                            MinorYN = "N";
                        }
                        string MinorDob = Convert.ToString( birthDate.Date);;
                        string Guardian = "";
                        string Issuingplace = "";
                        string Profession = response2.Profession;                       
                        byte[] data = resul1.DocumentHistory; 
                        string AdharNo = response1.AadharNo;
                        



                        bool? JointAccountYN = response2.IsJointHolder;//"N"; 
                        if (JointAccountYN == true)
                        {
                            test = "Y";

                        }
                        if (JointAccountYN == false)
                        {
                            test = "N";
                        }
                        string JointHolders = "";


                        string title1 = objAccount.FSubTitle;// "MR";// objAccount.FSubTitle;
                        string longname1 =  objAccount.firstapplicantFNAME + " " + objAccount.firstapplicantLNAME; //"mayur pacharbne";//"Prashant Patil"; 
                        string FatherName1 = objAccount.firstapplicantMNAME; //"dattu";//"Bibhishan";
                        string Add1a = objAccount.FJointAdd1;//"hhbxss";//response1.ClientPermAddress1;
                        string Add2b = objAccount.FJointAdd2;//;//objAccount.FJointAdd2;//response1.ClientPermAddress2;
                        string Add3c = objAccount.FJointAdd3;// "sdfghjkl";// objAccount.FJointAdd3;//response1.ClientPermAddress3;
                        string Area1 = objAccount.FCity;//"001";//objAccount.;
                        string City1 = objAccount.FCity;// "mumbai";//"";
                        string PinCode1 = objAccount.FPin;// "400071";//
                        string State1 = objAccount.FState; //"MAH";//
                        string Country1 = objAccount.FCountry;//// "IN";
                        string Nationality1 = "Indian";
                        string Mobile1 = objAccount.FMobileNo;//"9595954805"; //
                        string EmailID1 = objAccount.FEmailId; //;//objAccount.FEmailId; //"apb@gmail.com";//
                        string Introducer1 = "N";
                        string IntroducerCustomerNumber1 = "";
                        string PanNo1 = ObjTripleDes.Decrypt(objAccount.FPanNo); //"FOHPP6818B";//"BWGPS3701F"; 
                        string PanDesc1 = "";
                        string TDSYN1 = "";
                        string TDSReasonCd1 = "";
                        string Form15App1 = "";
                        string Date1 = "dd-MM-yyyy";
                        string DOB1 = objAccount.FDob; //"12-10-2001";//objAccount.FDob;
                        string SexCode1 = objAccount.FGender;// " ";//objAccount.FGender;
                        string CasteCd1 = objAccount.Caste1;//;//objAccount.Caste1;//"1";
                        string Religion1 = objAccount.Religion1;// " ";// "000000001";
                        string MaritalStatus1 = objAccount.MaritalStatus1;//" ";//"000000001";
                        string MarriageDate1 = "000000003";
                        string Noofchildren1 = "";
                        string Bloodgroup1 = "";
                        string Residence1 = objAccount.FResidence;//" ";// ;
                        string document1 = objAccount.FDocument; //" ";//
                        string CreditCardNumber1 = "";
                        string DateValid1 = "";
                        string ResidenceYN1 = objAccount.FResidenceYN; //;// 0000001";
                        string PassportNumber1 = objAccount.FPassportNo; //" ";// 
                        string ResidenceStatus1 = objAccount.ResidentialStatus1; //" ";////"000000001";
                        string VisaValidUpto1 = "";
                        string AccountType1 =response2.AccountType;// "SB";
                        string PhoneBankingYN1 = objAccount.FPhoneBAnking;// "N";//
                        string test1 = "";
                        string AmlRating1 = response1.AMLRating; //"1";//
                        string PhysicalChaYN1 = response2.PhysicallyChall_YN; //"Y";//response2.PhysicallyChall_YN;
                        string PhysicalChalDesc1 = response2.PhysicallyChall;// ""; //
                        string VerifiedDoc1 = response2.VerifyDOC; //"Aadhar/Pan";//
                        string AnnualIncome1 = response2.AnnualIncome;// "3LAc to 5LAc";// 
                        string Occupation1 = response2.OccupationType;// "SERVICE INDUSTRY";//
                        string Qualification1 = response2.EducationQualification; //"qwe";// 
                        string Age1 = response2.NomineeAge.ToString(); //"25";// 
                        string MinorYN1 = response2.Minor; //"";// 
                        string MinorDob1 = response1.Dob; //"";// response1.Dob;
                        string Guardian1 = "";
                        string Issuingplace1 = "";
                        string Profession1 = response2.Profession;//"7";//

                        string title2 = objAccount.SSubTitle;// "Mr";// // objAccount.PrimaryCustomerID;
                        string longname2 = objAccount.SecondapplicantFNAME + " " + objAccount.SecondapplicantLNAME;// "Shubham Batley";////"Shubham Dagle";
                        string FatherName2 = objAccount.SecondapplicantMNAME; //"Bappa ";//"Shalikram";// //objAccount.firstapplicantMNAME;
                        string Add1a2 = objAccount.SJointAdd1;//"asdf";//"House 01";//response1.ClientPermAddress1;
                        string Add2b2 = objAccount.SJointAdd2;// "qwer";// "Bldg 2";//response1.ClientPermAddress2;
                        string Add3c2 = objAccount.SJointAdd3;//"qwerew";// "Sai Nagar";//response1.ClientPermAddress3;
                        string Area2 = objAccount.SCity; //"02";// 
                        string City2 = objAccount.SCity; //"Mumbai";//
                        string PinCode2 = objAccount.SPin;// "400025";//.SPin;
                        string State2 = objAccount.SState;// "MAH";// 
                        string Country2 = objAccount.BirthPlaceCountry1; //"IN";// 
                        string Nationality2 = "Indian";
                        string Mobile2 = objAccount.SMobileNo; //"9595954805"; ////
                        string EmailID2 = objAccount.SEmailId; //"akhil@gmail.com";//"apb@gmail.com"; //
                        string Introducer2 = "N";
                        string IntroducerCustomerNumber2 = "";
                        string PanNo2 = ObjTripleDes.Decrypt(objAccount.SPanNo);// "BWGPT3701F";//"BWGPT3701F";//
                        string PanDesc2 = "d";
                        string TDSYN2 = "";
                        string TDSReasonCd2 = "";
                        string Form15App2 = "";
                        string Date2 = "dd-MM-yyyy";
                        string DOB2 = objAccount.SDob;// "1-01-1996";//  ;
                        string SexCode2 = objAccount.SGender; //"M";//
                        string CasteCd2 = objAccount.Caste1;//"1";//"1
                        string Religion2 = objAccount.Religion2;// "000000003";//
                        string MaritalStatus2 = objAccount.MaritalStatus2; //"000000003";// 
                        string MarriageDate2 = "000000003";
                        string Noofchildren2 = "";
                        string Bloodgroup2 = "";
                        string Residence2 = objAccount.SResidence;// "000000003";// ;//"000000003";
                        string document2 = objAccount.SDocument;// "000000003";//objAccount.SDocument;//"000000001";
                        string CreditCarNumber2 = "";
                        string DateValid2 = "";
                        string ResidenceYN2 = objAccount.SResidenceYN; // "Y";//0000001";
                        string PassportNumber2 = objAccount.SPassportNo;// " ";//
                        string ResidenceStatus2 = objAccount.ResidentialStatus1;// "000000003";//  "000000001";
                        string VisaValidUpto2 = "";
                        string AccountType2 = response2.AccountType;//"SB";// "SB";
                        string PhoneBankingYN2 = objAccount.SPhoneBAnking;// "N";//
                        string test2 = "";
                        string AmlRating2 = response1.AMLRating;// "1";
                        string PhysicalChaYN2 = response2.PhysicallyChall_YN; //"N";
                        string PhysicalChalDesc2 = response2.PhysicallyChall; //"NotBlind"; //
                        string VerifiedDoc2 = response2.VerifyDOC;// "Aadhar/Pan";//
                        string AnnualIncome2 = response2.AnnualIncome;// "3LAc to 5LAc";// 
                        string Occupation2 = response2.OccupationType;// "SERVICE INDUSTRY";// 
                        string Qualification2 = response2.EducationQualification;// "ssss";// 
                        string Issuingplace2 = "";
                        string Profession2 = response2.Profession;// "34";//

                        bool? JointHolder = response2.IsJointHolder;

                        if (JointHolder == true)
                        {

                            var client = new RestClient("https://cbsintegration.azurewebsites.net");


                            var request = new RestRequest("/api/SubmitFinacleDataForJointHolders?title=" + title + "&longname=" + longname + "&FatherName=" + FatherName + "&Add1=" + Add1 + "&Add2=" + Add2 + "&Add3=" + Add3 + "&Area=" + Area + "&City=" + City + "&PinCode=" + PinCode + "&State=" + State + "&Country=" + Country + "&Nationality=" + Nationality + "&Mobile=" + Mobile + "&EmailID=" + EmailID + "&Introducer=" + Introducer + "&IntroducerCustomerNumber=" + IntroducerCustomerNumber + "&PanNo=" + PanNo + "&PanDesc=" + PanDesc + "&TDSYN=" + TDSYN + "&TDSReasonCd=" + TDSReasonCd + "&Form15App=" + Form15App + "&Date=" + Date + "&DOB=" + DOB + "&SexCode=" + SexCode + "&CasteCd=" + CasteCd + "&Religion=" + Religion + "&MaritalStatus=" + MaritalStatus + "&MarriageDate=" + MarriageDate + "&Noofchildren=&Bloodgroup=&Residence=" + Residence + "&document=" + document + "&CreditCardNumber=&DateValid=&ResidenceYN=" + ResidenceYN + "&PassportNumber= &ResidenceStatus=" + ResidenceStatus + "&VisaValidUpto=&AccountType=" + AccountType + "&AMLRating=" + AmlRating + "&PhysicallyChaYN=" + PhysicalChaYN + "&PhysicalChalDesc=" + PhysicalChalDesc + "&AnnualIncome=" + AnnualIncome + "&Occupation=" + Occupation + "&VerifiedDoc" + VerifiedDoc + "&PhoneBankingYN=" + PhoneBankingYN + "&JointAccountYN=" + test + "&Name=" + NomName + "&Relationship=" + Relationship + "&Nadd1=" + NomAdd1 + "&Nadd2=" + NomAdd2 + "&Nadd3=" + NomAdd3 + "&Nadd4=" + NomAdd4 + "&Age=" + Age + "&MinorYN="+ MinorYN + "&MinorDob="+ MinorDob + "&Guardian=&Issuingplace=&Profession=" + Profession + "&EducationalQual=" + Qualification + "&JointHolders=" + JointHolder + "&title1=" + title1 + "&longname1=" + longname1 + "&FatherName=" + FatherName1 + "&Add1a=" + Add1a + "&Add2b=" + Add2b + "&Add3c=" + Add3c + "&Area1=" + Area1 + "& City1=" + City1 + "&PinCode1=" + PinCode1 + "&State=" + State1 + "&Country1=" + Country1 + "&Nationality1=" + Nationality1 + "&Mobile1=" + Mobile1 + "&EmailID1=" + EmailID1 + "&Introducer1=&IntroducerCustomerNumber1=&PanNo1=" + PanNo1 + "&PanDesc1=&TDSYN1=&TDSReasonCd1=&Form15App1=&Date1=" + Date1 + "&DOB1=" + DOB1 + "&SexCode1=" + SexCode1 + "&CasteCd1=" + CasteCd1 + "&Religion1=" + Religion1 + "&MaritalStatus1=" + MaritalStatus1 + "&MarriageDate1=&Noofchildren1=&Bloodgroup1=&Residence1=" + Residence1 + "&document1=" + document1 + "&CreditCardNumber1=&DateValid1=&ResidenceYN1=" + ResidenceYN1 + "&PassportNumber1=" + PassportNumber1 + "&ResidenceStatus1=" + ResidenceStatus1 + "&VisaValidUpto1=" + VisaValidUpto1 + "&AccountType1=" + AccountType1 + "&AMLRating1=" + AmlRating1 + "&PhysicallyChaYN1=" + PhysicalChaYN1 + "&PhysicalChalDesc1=" + PhysicalChalDesc1 + "&AnnualIncome1=" + AnnualIncome1 + "&Occupation1=" + Occupation1 + "&VerifiedDoc1=" + VerifiedDoc1 + "&Issuingplace1=&Profession1=" + Profession1 + "&EducationalQual1=" + Qualification1 + "&PhoneBankingYN1=" + PhoneBankingYN1 + "&title2=" + title2 + "&longname2=" + longname2 + "&FatherName2=" + FatherName2 + "&Add1a2=" + Add1a2 + "&Add2b2=" + Add2b2 + "&Add3c2=" + Add3c2 + "&Area2=" + Area2 + "&City2=" + City2 + "&PinCode2=" + PinCode2 + "&State2=" + State2 + "&Country2=" + Country2 + "&Nationality2=" + Nationality2 + "&Mobile2=" + Mobile2 + "&EmailID2=" + EmailID2 + "&Introducer2=" + Introducer2 + "&IntroducerCustomerNumber2=&PanNo2=" + PanNo2 + "&PanDesc2=&TDSYN2=&TDSReasonCd2=&Form15App2=&Date2=" + Date2 + "&DOB2=" + DOB2 + "&SexCode2=" + SexCode2 + "&CasteCd2=" + CasteCd2 + "&Religion2=" + Religion2 + "&MaritalStatus2=" + MaritalStatus2 + "&MarriageDate2=" + MarriageDate2 + "&Noofchildren2=&Bloodgroup2=&Residence2=" + Residence2 + "&document2=" + document2 + "&CreditCardNumber2=&DateValid2=&ResidenceYN2=" + ResidenceYN2 + "&PassportNumber2=&ResidenceStatus2=" + ResidenceStatus2 + "&VisaValidUpto2=&AccountType2=" + AccountType2 + "&AMLRating2=" + AmlRating2 + "&PhysicallyChaYN2=" + PhysicalChaYN2 + "&PhysicalChalDesc2=" + PhysicalChalDesc2 + "&AnnualIncome2=" + AnnualIncome2 + "&Occupation2=" + Occupation2 + "&VerifiedDoc2" + VerifiedDoc2 + "&Issuingplace2=&Profession2=" + Profession2 + "&EducationalQual2=" + Qualification2 + "&PhoneBankingYN2=" + PhoneBankingYN2, Method.POST);
                            request.AlwaysMultipartFormData = true;
                            string contentType = null;
                            request.AddFile("Signature", data, "ext", contentType);
                            request.AddFile("Signature1", firstjointsignature, "ext", contentType);
                            request.AddFile("Signature2", secondjointsignature, "ext", contentType);
                            IRestResponse response = client.Execute(request);
                            String Result = response.Content;
                            Result = Result.Replace(@"\", "");
                            String Result1 = Result.Replace(@"\", "");

                            string Response1 = Result1.Replace("{", "");
                            string Response2 = Response1.Replace("}", "");
                            string Response3 = Response2.Replace("[", "");
                            string Response4 = Response3.Replace("]", "");
                            string Response5 = Response4.Replace(":", ",");


                            var jsonSendResponse = Response5.Split(',', '"');
                            //var jsonSendResponse2 = jsonSendResponse.Split(',');

                            joint.Name = jsonSendResponse[5].ToString().Trim();
                            joint.Success = jsonSendResponse[28].ToString().Trim();
                            joint.CustomerNO = jsonSendResponse[11].ToString().Trim();
                            joint.AccountNo = jsonSendResponse[17].ToString().Trim();
                            joint.Name1 = jsonSendResponse[36].ToString().Trim();
                            joint.CustomerNO1 = jsonSendResponse[42].ToString().Trim();
                            joint.Name2 = jsonSendResponse[48].ToString().Trim();
                            joint.CustomerNO2 = jsonSendResponse[54].ToString().Trim();

                            if (joint.Success == "true")
                            {
                                using (SqlConnection connection2 = new SqlConnection(conn))
                                {
                                    SqlCommand cmd2 = new SqlCommand("Usp_ToInsertSILResponseDetailsforJointCustomers", connection2);
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.AddWithValue("@CustomerID", CustomerId);
                                    cmd2.Parameters.AddWithValue("@Account_no", joint.AccountNo);
                                    cmd2.Parameters.AddWithValue("@PrimaryCust_No", joint.CustomerNO);
                                    cmd2.Parameters.AddWithValue("@Success", joint.Success);
                                    cmd2.Parameters.AddWithValue("@PrimaryCustName", joint.Name);
                                    cmd2.Parameters.AddWithValue("@JointCustName1", joint.Name1);
                                    cmd2.Parameters.AddWithValue("@JointCustName2", joint.Name2);
                                    cmd2.Parameters.AddWithValue("@JoinCust_No1", joint.CustomerNO1);
                                    cmd2.Parameters.AddWithValue("@JoinCust_No2", joint.CustomerNO2);
                                    cmd2.Parameters.AddWithValue("@Branch", joint.Branch);
                                    connection2.Open();
                                    SqlDataReader reader2 = cmd2.ExecuteReader();

                                    if (reader2.Read())
                                    {
                                        using (SqlConnection connection4 = new SqlConnection(conn))
                                        {
                                            SqlCommand cmd4 = new SqlCommand("USP_UpdateSubmitToFinacleFlag", connection4);
                                            cmd4.CommandType = CommandType.StoredProcedure;
                                            cmd4.Parameters.AddWithValue("@CustomerId", CustomerId);
                                            connection4.Open();
                                            SqlDataReader reader4 = cmd4.ExecuteReader();
                                            if (reader4.Read())
                                            {
                                                //var resp1 = reader["FLAG"].ToString();

                                            }
                                        }
                                    }
                                }
                                return Json("Joint Account Created Successfully");
                            }

                        }
                        else
                        {
                            
                            var client = new RestClient("https://cbsintegration.azurewebsites.net");

                            //var client = new RestClient(options);
                            var request = new RestRequest("/api/SubmitFinacleData?title=" + title + "&longname=" + longname + "&FatherName=" + FatherName + "&Add1=" + Add1 + "&Add2=" + Add2 + "&Add3=" + Add3 + "&Area=" + Area + "&City=" + City + "=&PinCode=" + PinCode + "&State=" + State + "&Country=" + Country + "&Nationality=" + Nationality + "&Mobile=" + Mobile + "&EmailID=" + EmailID + "&Introducer=" + Introducer + "&IntroducerCustomerNumber=&PanNo=" + PanNo + "&PanDesc=&TDSYN=&TDSReasonCd=&Form15App=&Date=" + Date + "&DOB=" + DOB + "&SexCode=" + SexCode + "&CasteCd=" + CasteCd + "&Religion=" + Religion + "&MaritalStatus=" + MaritalStatus + "&MarriageDate=" + MarriageDate + "&Noofchildren=&Bloodgroup=&Residence=" + Residence + "&document=" + document + "&CreditCardNumber=&DateValid=&ResidenceYN=" + ResidenceYN + "&PassportNumber=&ResidenceStatus=" + ResidenceStatus + "&VisaValidUpto=&AccountType=" + AccountType + "&AMLRating=" + AmlRating + "&PhysicalChaYN=" + PhysicalChaYN + "&PhysicalChalDesc=" + PhysicalChalDesc + "&AnnualIncome=" + AnnualIncome + "&Occupation=" + Occupation + "&VerifiedDoc=" + VerifiedDoc + "&PhoneBankingYN=" + PhoneBankingYN + "&JointAccountYN=" + test + "&JointHolders=&Name=" + NomName + "&Relationship=" + Relationship + "&NAdd1=" + NomAdd1 + "&NAdd2=" + NomAdd2 + "&NAdd3=" + NomAdd3 + "&NAdd4=" + NomAdd4 + "&Age=" + Age + "&MinorYN="+ MinorYN + "&MinorDob="+ MinorDob + "&Guardian=&Issuingplace=&Profession=" + Profession + "&EducationalQual=" + Qualification + "&AdharNo=" + AdharNo + "&URN=" + CustomerID, Method.POST);
                            request.AlwaysMultipartFormData = true;
                            string contentType = null;
                            request.AddFile("Signature", data, "ext", contentType);
                            IRestResponse response = client.Execute(request);
                            Console.WriteLine(response.Content);
                            String Result = response.Content;
                            Result = Result.Replace(@"\", "");

                            string Response1 = Result.Replace("{", "");
                            string Response2 = Response1.Replace("}", "");
                            string Response3 = Response2.Replace(":", ",");
                            //if(Response2.Contains("timestamp")|| Result=="")
                            //{
                            //    return Json("No Response");
                            //}
                            var jsonSendResponse = Response3.Split(',', '"');
                            objroot.Success = jsonSendResponse[34].ToString().Trim();
                            objroot.Name = jsonSendResponse[5].ToString().Trim();
                            objroot.AccountNo = jsonSendResponse[17].ToString().Trim();
                            objroot.CustomerNO = jsonSendResponse[11].ToString().Trim();
                            objroot.Branch = jsonSendResponse[23].ToString().Trim();

                            if (objroot != null)
                            {
                                string Success = objroot.Success;
                                // string Accno = objroot.accNo;
                                string CustomerName = objroot.Name;
                                string CustomerNo = objroot.CustomerNO;
                                string AccountNo = objroot.AccountNo;
                                string Branch = objroot.Branch;
                                if (Success == "true")
                                {
                                    using (SqlConnection connection2 = new SqlConnection(conn))
                                    {
                                        SqlCommand cmd2 = new SqlCommand("Usp_toInsertSILResponseDetailsforCustomers", connection2);
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.AddWithValue("@CustomerID", CustomerId);
                                        cmd2.Parameters.AddWithValue("@Account_no", AccountNo);
                                        cmd2.Parameters.AddWithValue("@customer_No", CustomerNo);
                                        cmd2.Parameters.AddWithValue("@Success", Success);
                                        cmd2.Parameters.AddWithValue("@CustomerName", CustomerName);
                                        cmd2.Parameters.AddWithValue("@Branch", objroot.Branch);
                                        //cmd2.Parameters.AddWithValue("@Digilockertype", "PAN");

                                        connection2.Open();
                                        SqlDataReader reader2 = cmd2.ExecuteReader();
                                        if (reader2.Read())
                                        {
                                            //var ivar = reader["result"].ToString();
                                            using (SqlConnection connection4 = new SqlConnection(conn))
                                            {
                                                SqlCommand cmd4 = new SqlCommand("USP_UpdateSubmitToFinacleFlag", connection4);
                                                cmd4.CommandType = CommandType.StoredProcedure;
                                                cmd4.Parameters.AddWithValue("@CustomerId", CustomerId);


                                                connection4.Open();
                                                SqlDataReader reader4 = cmd4.ExecuteReader();
                                                if (reader4.Read())
                                                {
                                                    //var resp1 = reader["FLAG"].ToString();

                                                }
                                            }
                                        }
                                    }
                                    string PrimaryMobno = response1.MobileNo;
                                    string CustomerName1 = objroot.Name;
                                    string CustomerNo1 = objroot.CustomerNO;
                                    string AccountNo1 = objroot.AccountNo;
                                    string Branch1 = objroot.Branch;

                                    var client7 = new RestClient("https://cbs.indofinnet.com/api/AccountCreationSMSRSSB?AccountNO=" + AccountNo1 + "&CustomerID=" + CustomerNo1 + "&ToMobileNo=" + PrimaryMobno + "&Bankname=RSSB");


                                    client7.Timeout = -1;
                                    var request12 = new RestRequest(Method.GET);
                                    request12.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                    IRestResponse response12 = client7.Execute(request12);
                                    string res12 = response12.Content;

                                    res12 = res12.Replace(@"\", "");

                                    string s = res12.Split('"')[1];
                                    if (s == "Empty text not allowed, rejected.")
                                    {
                                        return Json(null);
                                    }
                                    else
                                    {
                                        return Json("Account Created Successfully");
                                    }
                                }

                                else
                                {
                                    return Json("Account Creation Unsuccessfull");

                                }
                            }
                            else
                            {
                                return Json("No Response");
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
            return Json("");
        
        }

        public ActionResult Rekyc()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var CustomerId = HttpContext.Session.GetString("PersonalId");
                string conn = _connectionString;
                using (SqlConnection connection11 = new SqlConnection(conn))
                {
                    SqlCommand cmd11 = new SqlCommand("USP_SavingAccountDetails", connection11);
                    cmd11.CommandType = CommandType.StoredProcedure;

                    cmd11.Parameters.AddWithValue("@CustomerId", CustomerId);
                    connection11.Open();
                    SqlDataReader reader = cmd11.ExecuteReader();
                    if (reader.Read())
                    {
                        //var response1 = objDetails.AdmKycCustomerDetail1s.FromSqlRaw($"USP_SavingAccountDetails {CustomerId}").AsEnumerable().FirstOrDefault(); //reader["result"].ToString();
                        //var response2 = objDetails.AdmSilResponseDetails.FromSqlRaw($"USP_RekycAccCNO {response1.CbsId}").AsEnumerable().FirstOrDefault(); //reader["result"].ToString();

                        //string CustomerNumber = response1.CbsId; //"622080";
                        //string AcctNo = response2.Account_no;//"001215000000011";
                        var response1 = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_SavingAccountDetails {CustomerId}").AsEnumerable().FirstOrDefault(); //reader["result"].ToString();

                        string CustomerNumber = response1.CbsId;// "622080";
                        string AcctNo = response1.Account_No;//"001215000000011";

                        if (response1.CountryId == "101")
                        {
                            HttpContext.Session.SetString("Country", "IN");
                        }

                        string ADD2= response1.ClientAddress2;
                        if (ADD2 != null || ADD2 != "")
                        {
                            ADD2 = response1.ClientAddress2;
                        }
                        else
                        {
                            ADD2 = "";
                        }


                        string PermAdd1 = response1.ClientAddress1; //"\"House 01\""; //
                        string PermAdd2 = response1.ClientAddress2; //"\"Bldg 2\"";// 
                        string PermAdd3 = response1.ClientAddress3; //"\"Sai Nagar\"";// 
                        string PermArea = response1.StateId;  //"\"001\"";//
                        string PermCity = response1.StateId; //"\"\"";//
                        string PermPinCode = response1.PinCode;
                        string PermCountry = HttpContext.Session.GetString("Country");//response1.CountryId; //"\"IN\"";//
                        string TempAdd1 = response1.ClientAddress1; //"\"House 01\"";//
                        string TempAdd2 = response1.ClientAddress2; //"\"Bldg 2\"";//
                        string TempAdd3 = response1.ClientAddress3;  //"\"Sai Nagar\"";//
                        string TempArea = response1.StateId; //response1.ClientPermCity;  //"\"001\"";//
                        string TempCity = response1.StateId;// response1.ClientPermCity;  //"\"\"";//
                        string TempPinCode = response1.PinCode;
                        string TempCountry = HttpContext.Session.GetString("Country"); //response1.CountryId; //"\"IN\""; //

                        var client = new RestClient("https://cbs.indofinnet.com/api/AddressUpdateREKYC?CustNo=" + CustomerNumber + "&AcctNo=" + AcctNo + "&PrenAdd1=" + PermAdd1 + "&PrenAdd2=" + PermAdd2 + "&PrenAdd3=" + PermAdd3 + "&PrenArea=" + PermArea + " &PrenCity=" + PermCity + "&PrenPinCode=" + PermPinCode + "&PrenCountry=" + PermCountry + "&TempAdd1=" + TempAdd1 + "&TempAdd2=" + TempAdd2 + "&TempAdd3=" + TempAdd3 + "&TempArea=" + TempArea + "&TempCity=" + TempCity + "&TempPinCode=" + TempPinCode + "&TempCountry=" + TempCountry);
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("BankCode", "RSSB");
                        request.AddHeader("Authorization", "Basic c2lsQDEyMzpTaWxAMTIz");
                        var body = @"";
                        request.AddParameter("application/json", body, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);

                        var res = response.Content;

                        if (!string.IsNullOrEmpty(res))
                        {
                            String Result = response.Content;
                            Result = Result.Replace(@"\", "");
                            Result = Result.Substring(1, Result.Length - 2);
                            RekycClass objroot = JsonConvert.DeserializeObject<RekycClass>(Result);
                            if (objroot != null)
                            {
                                string responseRekyc = objroot.Message;
                                if (responseRekyc == "Address Updated Successfully ")
                                {

                                    using (SqlConnection cn = new SqlConnection(_connectionString))
                                    {
                                        cn.Open();
                                        SqlCommand cmd = new SqlCommand("USP_InsertRkycFinal", cn);

                                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@CId", HttpContext.Session.GetString("PersonalId"));
                                        cmd.Parameters.AddWithValue("@CustId", CustomerNumber);
                                        cmd.Parameters.AddWithValue("@CustAccNo", AcctNo);
                                        cmd.Parameters.AddWithValue("@CustFnm", response1.FirstName);
                                        cmd.Parameters.AddWithValue("@CustMnm", response1.MiddleName);
                                        cmd.Parameters.AddWithValue("@CustLnm", response1.LastName);
                                        cmd.Parameters.AddWithValue("@status", responseRekyc);

                                        cmd.ExecuteNonQuery();
                                    }
                                    string conn1 = _connectionString;

                                    using (SqlConnection connection3 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd3 = new SqlCommand("USP_RKYCAddressFlag", connection3);
                                        cmd3.CommandType = CommandType.StoredProcedure;
                                        cmd3.Parameters.AddWithValue("@CustId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                        connection3.Open();
                                        SqlDataReader reader3 = cmd3.ExecuteReader();
                                        if (reader3.Read())
                                        {
                                            //var Result = reader2["RESULT"].ToString();
                                        }
                                    }
                                    return Json(responseRekyc);
                                }
                            }
                            else
                            {
                                return Json("Error in Address Updation");
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
            return Json("Please Try After Sometime");
        }

        public ActionResult JointFlagforAgent()
        {
            var  CustomerID = HttpContext.Session.GetString("PersonalId");
            using (SqlConnection connectStr = new SqlConnection(_connectionString))
            {
                connectStr.Open();
                SqlCommand cmd = new SqlCommand("USP_JointFlagAgentUpdate", connectStr);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustId", CustomerID);


                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("UserDetails", "AdminLogin"); 
        }

        public JsonResult GetStateAndDistrictByPINCode(string countrycount)
        {
            var pin = objDetails.sysPinAndStateMasters.Where(a => a.Pin_Code == countrycount).FirstOrDefault();
            if (pin != null)
            {

                return new JsonResult(pin);
            }
            else
            {
                return Json("pincode not valid");
            }
        }
    }
}

