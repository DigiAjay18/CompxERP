using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 

namespace CompxERP.Models
{
    public class EmployeeSetup
    {
        public string sSysName { get; set; }
          [RegularExpression(@"[0]?[789]\d{9}", ErrorMessage = "Enter Valid Mobile No.")]
        public string sPhone { get; set; }
          [RegularExpression(@"[0]?[789]\d{9}", ErrorMessage = "Enter Valid Mobile No.")]
          public string sMobile { get; set; }
        public string sRema { get; set; }
        public string sCustHindi { get; set; }
        public string sFatherHindi { get; set; }

        public string sPAN { get; set; }
        public string sTIN { get; set; }
        public string sCST { get; set; }
        public string sTAN { get; set; }
        public string sRemark { get; set; }
        public int iCompCode { get; set; }
        public int iMstType { get; set; }
        public int iMstCode { get; set; }
        public Nullable<DateTime> dDate { get; set; }
        public DateTime dIncDate { get; set; }
        public int iCity { get; set; }
        public int iWard { get; set; }
        public int iColony { get; set; }
        public int iZone { get; set; }
        public int iAgreeterm { get; set; }
        public int iTerm { get; set; }
        public int iPayMode { get; set; }
        public int iRentType { get; set; }
        public decimal dRent { get; set; }
        public decimal dLateFee { get; set; }

        public int iEmp_empcode { get; set; }
        public int iEmp_empcity { get; set; }
        public int iEmp_empledg { get; set; }
        public int iEmp_empgrou { get; set; }
        public int iEmp_empitrd { get; set; }
        public int iEmp_empdsa { get; set; }
        public int iEmp_empbudget { get; set; }
        public int iEmp_empincome { get; set; }
        public int iEmp_emparea { get; set; }
        public int iEmp_empnomi { get; set; }
        public int iEmp_empdsap { get; set; }
        public bool iEmp_empprty { get; set; }
        public int iEmp_empcate { get; set; }
        public int iEmp_handicap { get; set; }
        public string sEmp_empname { get; set; }
        public string sEmp_empalia { get; set; }
        public string sEmp_empaddr { get; set; }
        public string sEmp_empstat { get; set; }
        public string sEmp_empphon { get; set; }
        public string sEmp_emprema { get; set; }
        public string sEmp_emppath { get; set; }
        public string sEmp_empsort { get; set; }
        public string sEmp_emppinc { get; set; }
        public string sEmp_emprefn { get; set; }
        public string sEmp_emphind { get; set; }

        public decimal dEmp_empopdr { get; set; }
        public decimal dEmp_empopcr { get; set; }
        public decimal dEmp_empcldr { get; set; }
        public decimal dEmp_empclcr { get; set; }
        public decimal dEmp_empjmbl { get; set; }
        public decimal dEmp_empagen { get; set; }
        public decimal dEmp_acwithbl { get; set; }
        public decimal dEmp_emprate { get; set; }


        public int iEAcc_acctcode { get; set; }
        public int iEAcc_emposrno { get; set; }
        public int iEAcc_empopano { get; set; }
        public int iEAcc_empobuhe { get; set; }
        public int iEAcc_empopost { get; set; }
        public int iEAcc_empodesg { get; set; }
        public int iEAcc_empovvht { get; set; }
        public int iEAcc_emposexd { get; set; }
        public int iEAcc_empidpfc { get; set; }
        public int iEAcc_empbnknm { get; set; }
        public int iEAcc_empovarg { get; set; }
        public int iEAcc_emppfbnk { get; set; }
        public int iEAcc_empoccla { get; set; }

        public string sEAcc_empoaddr { get; set; }
        public string sEAcc_empobano { get; set; }
        public string sEAcc_empogpno { get; set; }
        public string sEAcc_empocpno { get; set; }
        public string sEAcc_empopann { get; set; }
        public string sEAcc_empidprf { get; set; }
        public string sEAcc_empesino { get; set; }
        public string sEAcc_empprfno { get; set; }

        public decimal dEAcc_empobasl { get; set; }
        public decimal dEAcc_empolicn { get; set; }

        public Nullable<DateTime> dEAcc_empodoan { get; set; }
        public Nullable<DateTime> dEAcc_empojodt { get; set; }
        public Nullable<DateTime> dEAcc_empobidt { get; set; }
        public Nullable<DateTime> dEAcc_empoindt { get; set; }

        public string dEAcc_empojodts { get; set; }
        public string sEmp_PAN { get; set; }
        public string sEmp_ESI { get; set; }
        public string sEmp_IDProof { get; set; }
        public string sEmp_ProofNo { get; set; }
        public string sEmp_AcNo { get; set; }
        public string sEmp_PfAcNo { get; set; }
        public string PfAcNo { get; set; }
        public string IFSC { get; set; }
        public string sEmp_VetanMaan { get; set; }

        public Nullable<DateTime> dEAcc_BirthDt { get; set; }
        public Nullable<DateTime> dEAcc_AnvDt { get; set; }

        public string dEAcc_BirthDts { get; set; }
        public int iEmp_Branch { get; set; }
        public int iEmp_Gender { get; set; }
        public int iEmp_CastID { get; set; }
        public int iEmp_CategoryID { get; set; }

        public int BankID { get; set; }
        public int iEmp_BankID { get; set; }
        public int iEmp_pfBankID { get; set; }

        public int iSapuMst_msttype { get; set; }
        public int iSapuMst_mstcode { get; set; }
        public int iSapuMst_mstsite { get; set; }
        public int iSapuMst_mstcust { get; set; }
        public decimal iSapuMst_mstlate { get; set; }//sourabh 2-nov-2015
        public int iSapuMst_mstAppr { get; set; }
        public int iSapuMst_mstptcode { get; set; }

        public string sSapuMst_mstrema { get; set; }
        public string sSapuMst_mstrefc { get; set; }
        public string sSapuMst_mstchno { get; set; }

        public string sSapuMst_mststatus { get; set; }

        public decimal dSapuMst_oldwht { get; set; }
        public decimal dSapuMst_oldamt { get; set; }
        public decimal dSapuMst_msttota { get; set; }

        public decimal dSapuMst_mstneta { get; set; }
        public decimal dSapuMst_mstpdc { get; set; }

        public Nullable<DateTime> dSapuMst_mstdate { get; set; }
        public Nullable<DateTime> dSapuMst_mstcfdt { get; set; }


        public int iSapuTrn_trntype { get; set; }
        public int iSapuTrn_trncode { get; set; }
        public int iSapuTrn_trnitem { get; set; }
        public int iSapuTrn_trnrefc { get; set; }
        public int iSapuTrn_trnshow { get; set; }
        public int iSapuTrn_trnsrno { get; set; }

        public string sSapuTrn_trnrema { get; set; }

        public decimal dSapuTrn_trnadju { get; set; }
        public decimal dSapuTrn_trndram { get; set; }
        public decimal dSapuTrn_trncram { get; set; }
        public decimal dSapuTrn_trnexpa { get; set; }
        public decimal dSapuTrn_trnaddv { get; set; }
        public decimal dSapuTrn_trnactpay { get; set; }

        public string acknowNo { get; set; }
        public string ReceiptNo { get; set; }

        public Nullable<DateTime> dSapuTrn_trndate { get; set; }
        public Nullable<DateTime> dSapuTrn_trnrefd { get; set; }

        public int iTypee_opbal { get; set; }
        public int iPrev_opbal { get; set; }
        public int iCurr_opbal { get; set; }
        public DateTime dDatee_opbal { get; set; }

        public Nullable<DateTime> dStDate { get; set; }
        public Nullable<DateTime> dLastDate { get; set; }

        public int msttype { get; set; }
        public int mstcode { get; set; }
        public DateTime mstdate { get; set; }
        public string msttota { get; set; }
        public string mstrema { get; set; }
        public string msttotb { get; set; }
        public string mstdesg { get; set; }
        public string mstDept { get; set; }
        public string mstcate { get; set; }
        public string mstempo { get; set; }
        public string mstapdt { get; set; }
        public string mstlvbsc { get; set; }
        public string mstlvbase { get; set; }
        public int mstshow { get; set; }
        public string msthour { get; set; }
        public string mstlvsy { get; set; }
        public string mstlvsm { get; set; }
        public string mstincd { get; set; }
        public string mstHindirema { get; set; }
        public string mstLotCode { get; set; }
        public int iChgValue { get; set; }
        public int iMstCodeV { get; set; }
        public int iUpdHeadC { get; set; }
        public string sUpdOnWht { get; set; }
        public string sAddOrSub { get; set; }
        public string sOperator { get; set; }
        public int iEmpLoan_Code { get; set; }
        public int iEmpLoan_CR { get; set; }
        public int iEmpLoan_DR { get; set; }
        public int iEmpLoan_Mode { get; set; }
        public int iEmpLoan_EmpCode { get; set; }
        public int iEmpLoan_Installment { get; set; }
        public int iEmpLoan_PaidInstall { get; set; }
        public int iEmpLoan_BalInst { get; set; }
        public int iEmpLoan_AdjuIntallment { get; set; }
        public string sEmpLoan_Challan { get; set; }
        public string sEmpLoan_remark { get; set; }
        public decimal dEmpLoan_AdjuAmt { get; set; }
        public decimal dEmpLoan_Amount { get; set; }
        public decimal dEmpLoan_Interest { get; set; }
        public decimal dEmpLoan_interestAmt { get; set; }
        public decimal dEmpLoan_Total { get; set; }
        public decimal dEmpLoan_PaidAmt { get; set; }
        public decimal dEmpLoan_InstallmentAmt { get; set; }
        public decimal dEmpLoan_BalAmt { get; set; }
        public decimal InstallAmtInt { get; set; }
        public int PaidInstallInt { get; set; }
        public decimal TotInt { get; set; }
        public decimal PaidAmtInt { get; set; }
        public decimal IntPer { get; set; }
        public int IntAccount { get; set; }
        public Nullable<DateTime> dEmpLoan_Date { get; set; }
        public Nullable<DateTime> dEmpLoan_FromMonth { get; set; }
        public Nullable<DateTime> dEmpLoan_LoanMonth { get; set; }
        public string sCheckNo { get; set; }
        [RegularExpression(@"[0]?[789]\d{9}", ErrorMessage = "Enter Valid Mobile No.")]
        public string sOfficial_No { get; set; }
        [RegularExpression(@"[0]?[789]\d{9}", ErrorMessage = "Enter Valid Mobile No.")]
        public string sResidential_No { get; set; }
        public string sReferenceNm { get; set; }
        [RegularExpression(@"[0]?[789]\d{9}", ErrorMessage = "Enter Valid Mobile No.")]
        public string sReferenceNo { get; set; }
        [RegularExpression(@"[0]?[789]\d{9}", ErrorMessage = "Enter Valid Mobile No.")]
        public string sFatherNo { get; set; }
        public string sTemporaryAdd { get; set; }
        public string sPermanentAdd { get; set; }
        public string sOld_PF_No { get; set; }
        public string sUNA_No { get; set; }
          [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Enter Valid Email ID")]
        public string sEmailID { get; set; }
          public string sBloodGroup { get; set; }
        public string Aadhar { get; set; }
        public string Father { get; set; }
        public string DegreeNM { get; set; }
        public int DegreeID { get; set; }
        public int iEmpLoan_Type { get; set; }
        public int iPlanType { get; set; }
        public int iPlanCode { get; set; }
        public string sDdo { get; set; }

        public string IMG_Path { get; set; }
        public bool AadharCard  {get; set;}
        public bool  VoterId {get; set;}
        public bool  Licence {get; set;}
        public bool Passport { get; set; }
        public bool  Statement {get; set;}
        public bool Degree { get; set; }
        public bool isEdit { get; set; }

        public HttpPostedFileWrapper ImageFile { get; set; }

        public int UseCode { get; set; }
        public string sPath { get; set; }
        public string sEmpUser { get; set; }
        public string sEmpPass { get; set; }
        public string sEmpRetype { get; set; }
        public List<EmployeeSetup> lstEmployeeSetup { get; set; }
        public string empUserType { get; set; }
    }
}