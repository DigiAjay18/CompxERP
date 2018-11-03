using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ConstVariable
{
    public enum repoType
    {
        ELEDGER = 0,
        ERBILLS = 1, //previously just in AI and CP now in ALL 20-2-04
        ERDlDsR = 2, //DAILY DESPATCH REPORT
        ERJOURN = 3, //JOURNAL REPORT
        ERCASH = 4,
        ERVOUC = 5,
        ERDAYB = 6,
        //
        EACCLI = 7, //For Account List
        ERDLRP = 8,
        ////////////////////////////////////////////anu
        ERMCRp = 50, //medium change
        ERSCRp = 51, //school change
        ERSBDL = 52, //student birthday
        ERSIDC = 53, //student id card
        ERTBDL = 54, //employee birthday
        EREMID = 55, //employee id card
        ERSFTP = 56, //summary fee paid list
        ////////////////////////////

        ETRBAL = 100,
        EBALSH = 101,
        EPNLAC = 102,
        ETRADI = 103,

        //madhvi
        ertdsr = 104,

        //in CosMos
        ERStSu = 203, //Stock Summary
        EStPre = 204, //Stock Preview - Custom Designed Report

        //in Bajaj
        EStRep = 205, //Stock Report

        //in Malani
        ELetPr = 206, //Letter Preview

        //in Reminder
        ERemiL = 207, //Reminders List


        //in Bajaj
        EPORDU = 208, //Order Due List purchase bill wise
        ESORDU = 209, //Order Due List bill wise
        ESTOCB = 210, //Stock Balance
        ECOUPR = 211, //Coupon Report

        //in SK
        ELABIS = 212, //Labour Information

        //in BJ
        EITSOR = 213, //Item Wise Sales Order List
        ESTBIF = 214, //Stock Bifercation
        ESTORP = 215, //Stock Report % wise

        EDCASH = 216, //Daily Cash Report

        EFISTO = 217, //Finished Stock List With Due Orders

        EPREVB = 218, //Previous Year Balance

        EBIFOP = 219, //Bifercation <=> Op. Stock
        EBILSU = 220, //Bills Summary
        EKHANS = 221, //Khand Wise Summary of Stock

        ELABEL = 222, //Labels List

        //IN SK
        ESUMMB = 223, //SUMMARY OUTLET WISE

        //IN AG
        EPLANR = 224, //PLANT REGISTER

        //  //in ALL
        //  ESTOUR = 225, ///Item Going Out
        //  ESTINR = 226, ///Item Coming In

        //in USER modules
        ERUSUM = 22, //uSER SUMMARY

        //stock summary starts from groups
        EGSUMM = 226,
        EVOUCL = 227,

        //IN mm
        ERAcSu = 228, //Account Summary

        //in AG
        EBaReS = 229, //Bank Reconciliation

        //in AITA
        EMIStat = 230, //MIS statement

        //in AG
        EBookOrd = 231,

        //in MM
        EItProfS = 232, //Item Profit Statement

        //in SK for COT
        ESOSPend = 233,//Sale Patti Pending List
        EInsuReg = 234,

        //in AG
        ERateReg = 235,//rate diff. report

        //in SK for HRW
        ERRefDet = 236, //ledgers as per billref details
        //in SK for COT
        ERspaPDU = 237, //spare purchase order due

        //in MP
        EItSOrPW = 238, //item sales order due list party wise

        //in SK for COT
        ERsosDue = 239, //S.O.S. bill due list
        ERsparSt = 240, //Spare Stock Reg.

        //in KA
        ERusedGt = 241, //Used/Not Used Summ
        //Of no use but to prevent errors 18-11-02
        //Comes in use in baba 20-11-02

        //in SK for tea
        ERdueBiL = 242, //due bills list horizontaly

        //in SK for transport
        ERarriLi = 243,
        ERdespLi = 244,
        ERbookLi = 245,
        //for aita
        ERDlDsNe = 246,

        //in SK for investment
        ERinveLi = 247,
        ERshareL = 248,

        //in SK for contract
        ERsiteLi = 249,
        ERcardLi = 250,
        ERreadLi = 251,

        //in SK for reportAs
        ERchartL = 252,

        //in SK for investor
        ERnavLis = 253, //to view date wise nav record
        ERnavFil = 254, //for list as per customers

        //in SK for reportAs
        ERprofit = 1255,
        ERLiability = 1256,
        ERasset = 1257,
        ERCashflw = 1258,
        ERAnnexure = 1259,
        ERPrjCost = 1260,

        //in SK for contract
        ERstafLi = 255,
        ERlorryB = 256,
        ERworkBo = 257,
        ERdieseB = 258,
        ERweekBo = 259,
        ERtranBo = 260,//transport book

        //-----------
        //in SK for reportAs
        ERPayMent = 261,
        ersales = 262, //Sales
        ERStock = 263, //Stock
        ERRMaterial = 264, //Raw Material
        ERPMaterial = 265, //Packing Material
        ERdepri = 266, //Rates Of Depriation
        ErMCB = 267, //Details Of machine & Building

        ERPaDuBo = 270, //payment due book
        ERbhadaR = 271, //bhada register as same as stock reg. but show only bhada amt.
        ERacWitR = 272, //account with item details for payment due
        ERmesDue = 273, //mesaurement sheet due
        //
        ERnDueLi = 274, //new due list
        //
        ERsretDu = 275, // sale ret. due
        ERpretDu = 276, // purchase ret. due
        //
        ERdayWoB = 277, // daily work book
        ERdStocB = 278, // stock book with daily option
        //
        ERconLab = 279, // labour reg.
        ERconWor = 280, // work reg.
        ERlaboBo = 281, // labour book

        ERmanuAc = 282, /// Manufacturing a/c

        ERcrDueL = 283, /// creditors due list
        ERtrDetB = 284, ///transporter detail book
        //
        ERhsLabR = 285, ///labour reg. for anh
        ERPWstoR = 286, ///party wise stock reg.
        ERIssReR = 287, ///issue and receipt reg.
        ERformSR = 288, ///formula wise stock reg
        ERreordR = 289, ///reorder status
        ERmoSuSL = 290, ///monthly summary as per sales
        ERmoSuPL = 291, ///monthly summary as per purchase
        //
        ERlabIDu = 292, ///labour issue due
        ERitePDu = 293, ///item wise payment due

        ERPWstBo = 294, ///customer wise stock book
        ERPWpuDu = 295, ///party wise p.o. due
        ERPWsaDu = 296, ///party wise s.o. due

        ERsaleDu = 297, ///sale bill due
        ERpurcDu = 298, ///purchase bill /ready for return due

        ERdaybSu = 299, ///day book summary
        ERPWstAc = 300, ///party wise stock  + a/c book

        ERExpDat = 301, ///expiry date status

        ERspInqD = 302, ///spare inquiry due
        ERspBala = 303, ///spare balance summar
        ERspDetR = 304, ///spare stock reg.

        ERtaxRep = 305, ///tax liability reports mandi tax
        //306, ///

        ERmacPnL = 307, ///machine p&l
        ERcostSh = 308, ///cost sheet

        ERlaboSu = 309, ///labour summary

        ErStDueL = 310, ///student due  list
        ErStRecL = 311, ///student rec. list
        ErMarksF = 312, ///marks feeding

        ErItDaSu = 313, ///item daily summary

        ERPtoCom = 314, ///party to company replacement
        ERCtoPar = 315, ///company to party replacement

        ErTeachR = 316, ///teachers record
        ErBaleSu = 318, ///bales summary
        ErBaDaSu = 319, ///bales daily summary

        ERlaboLi = 320, ///labour list
        ErOvMark = 321, ///overall marks list

        ErHdDueL = 322, ///head wise due list

        ERFeeDWo = 323, ///fees daily work book
        ERhsLabP = 324, ///labour pay bill

        ERpartSt = 325, ///party wise statement for checking of entries
        ERremakD = 326, ///remaking due

        ErListMk = 327, ///list of marks subject wise

        ErGRdetR = 328, ///G.R.Status Report

        ErAdmLis = 329, ///Admission List

        ErMulLed = 330, ///multi company ledger for BABA

        ErNewLed = 331, ///new ledger format

        ErSpecLe = 332, ///father wise ledger for BABA

        ErInsCrS = 333, ///insurance cross report for lawyer
        ERitPriL = 334, ///item wise price list (just only last price)

        ErCaseLi = 335, ///cases list

        ErMarkNe = 336, ///mark list for sr. secondary

        //FOR TRADER AC
        ErPendLo = 337, ///pend lot summary
        ErDetLot = 338, ///pend lot details
        ErCustAc = 339, ///customer A/c
        ErPeItSu = 340, ///pend item wise summary
        ErExchAc = 341, ///exchange a/c

        ErItWidR = 342, ///item wide report with packing and qty.

        ErHoriSu = 343, ///horizontally summmary of a/c
        ErGRcomR = 344, ///delivery compare report

        ErBusiVa = 345, ///business value report
        ErRegMem = 346, ///registered members list
        ErPrdReq = 347, ///product requirement

        ErTreeOF = 348, ///old format tree

        ErBankSt = 349, ///bank slip/statement

        ERIncPay = 350, ///incentive pay out for netmX
        ERsosLis = 351, ///sos bills details

        ErScheSl = 352, ///scheme sales detail

        ErAcItRg = 353, ///a/c item reg.

        ErSampIs = 354, ///sample issue reg. in depoX
        ErBuVSum = 355, ///business value summary
        ErCoupVS = 356, ///coupon nos. bal. summary

        ErMarkNu = 357, ///mark list for nursery
        ErExamtt = 358, ///Exam Time Table
        ErFeePlan = 359, ///Fee Plan
        ErSalaryP = 360, ///Salary Plan

        ErMarkPl = 361, ///Mark Plan
        ErStaffA = 362, ///Staff Attendance
        ErStudAt = 363, ///Student Attendance

        ErItRatR = 364, ///item stock report with rate column

        ErVATreg = 365, ///vat register

        ErDownCo = 366, ///down line list count

        ErPrRawC = 367, ///raw material consumption in production

        ErLotRep = 368, ///lot wise or reel wise report or bar code wise
        EaiDespL = 370, ///despatch list for aita

        ErPairBo = 371, ///pairs count book for happy india
        ErLevWLi = 372, ///level wise list of members
        ErJoinLi = 373, ///joined members list

        ErStPerW = 374, ///stock period wise

        ErBarCoI = 375, ///bar code report item wise

        ErPayOMe = 376, ///payout member wise

        ErPricLi = 377, ///price list entry record

        ErPayoPr = 378, ///payout print
        ErLegSum = 379, ///leg  summary

        ErDepBVE = 380, ///depo wise business value
        ErTDSumm = 381, ///tds summary
        ErBVDown = 382, ///b.v. down line list
        ErBVEarn = 383, ///b.v. earn list

        ErUsWork = 384, ///user work report

        ErPkClLi = 385, ///package cleared list
        ErPkSvLi = 386, ///package cleared and saved list

        ErExStFm = 387, ///stock report for excise format

        //for elex
        ErEmpPrd = 388, ///employee production wise
        ErFinDue = 389, ///finance due list

        ErExWtFm = 390, ///weight report for excise format
        ErRG23Ad = 391, ///rg 23

        //for pax
        ErBookOr = 392, ///bookin order list with packing unit and date/party wise summary
        ErVATsum = 393, ///vat computation
        ErEntXLi = 394, ///entry tax list
        ErNBReLi = 395, ///new bank reconcile
        ErAcPerW = 396, ///account period wise
        ErVoParS = 397, ///voucher wise party summary

        //for sch
        ErAdmFSu = 398, ///admission form summary
        ErFirecS = 399, ///form issue/rec. summary
        ErClWPdB = 400, ///class wise fee paid book

        //for pax
        ErMRPvLi = 401, ///M.R.P. value list
        ErProdDu = 402, ///prod. due for packing

        //for swapna
        ErLDBcal = 403,
        ErDRPdet = 404, ///daily reward point
        ErMemPyD = 405, ///member wise payout detail

        //for netwx
        ErCLBpay = 406, ///club payout
        ErBankWB = 407, ///cash/bank work book

        //for finax
        ErPCuStD = 451, ///customer detail report

        //for ConsX
        ErCCIReg = 470,
        ErBilty = 471,
        ErVehLed = 472,
        ErDricash = 473,
        ErInward = 474,  //----- for inward report
        ErVehExp = 475,  //----- for vehicle Expense Report
        ErFuelEx = 476,  //----- for fuel slip report
        ErDriExp = 477,   //----- for maintain detail of Driver Petty cash and Expense through that
        ErOutward = 478,
        ErPetrolDet = 479,
        ErCardChit = 480, ///************** shashwati 03/12/08
        ErVehUse = 481, ///********* shashwati 04/12/08, ///for vehicle use report
        ErVehPnl = 483, ///shashwati 25/12/08
        Erfuelslip = 484,
        //************** shashwati 5/1/09
        ErExpSumm = 485, ///summary report of vehicle expenses
        ErVehExSum = 486, ///shashwati 16/01/09

        //******************anubha
        ERepCTDS = 482, ///for tds report

        //for Payrx on 22-11-08 shashwati
        ErPaySumm = 483,
        ErSalryS = 485,
        ErPTrp = 486,
        ErLICrp = 487,
        //shashwati 29-11-08
        ErSalaryRp = 484,

        //for ChemX
        ErExPerf = 488,
        ErArAlot = 489,
        ErExpyDt = 490,
        ////////////////////////////////////////13-10-08
        ErPacwis = 492, ///for pack wise target report
        ErBetWis = 493, ///for bite wise target report
        ErProdTar = 494, ///for Product wise staff wise target achievment //
        ErCDWsPSh = 495, ///for CD wise party scheme//////////15/10/08
        ErPWsCDSh = 496, ///For Party Wise  CD Scheme
        ErMngPrd = 497, //for Manager wise Prod wise Sales and achieved target
        ErPropr = 498,

        //madhvi
        Ertdsdu = 499,
        ErTDSpay = 500,

        ErMontLe = 501, ///monthly ledger
        ErLandRt = 502, ///landing rate feeding

        //----anu for brk
        ErpFodSt = 510,
        ErpFodBl = 511,
        ErpFodSu = 512,
        //-----
        //********* Madhvi for vatx
        Erbkaval = 556,
        Erbkdefs = 557,
        Erbkissu = 558,
        Erbkrepo = 559,
        Erbkretn = 560,

        //******* shashwati on 10-11-08
        ErGoodsR = 513,
        //*************** shashwati on 22-12-08
        ErProdEx = 566, ///Product wise executive target
        ////****** madhvi
        Erwinst = 567,
        //************ Shashwati on 20-01-08 for compac
        ErStocL = 568,
        //********* shashwati 30-12-08
        ErPInvice = 569,
        //************
        //******** madhvi 14-1-09
        ErPrtwsS = 570,
        ErPrtsum = 571,
        ErCompsum = 572,
        //**************** Shashwati 20-01-09 for chemx
        ErGiftList = 573,
        ErExeCust = 574, ///******* Shashwati 23-01-09
        ErItemStoc = 575, ///****** Shashwati 24-01-09

        //***************** shashwati 24-12-08
        ErProdSum = 576,
        //***
        //**************** madhvi 24-12-08
        ErProdin = 577,
        //***
        ErLeaves = 588,
        //umang
        ErFollowUp = 541, ///for follow Up repo

        //madhvi
        Erfarpro = 551, ///form F
        ErSpissu = 552, ///spare issue
        Erchalpy = 553, ///tds challan

        ErEMDList = 655, ///////// Shashwati 05-04-09

        ErPayList = 656, /////Shashwati 08-04-09
        ErEMDParty = 657,  //shashwati 24-04-09  //EMD for MCPL To party
        ErPartyBl = 658, ///Shashwati 01-06-09  //Balance with Party
        ErPartyPyL = 659, ///Shashwati 02-06-09, ///Advance Payment List For MCPL to Party
        ErEsiMont = 660, ///shashwati 14-10-2010, ///For ESI monthly report for nitco
        ErEsiStn = 661, ///shashwati 19-10-2010, ///For ESI report station wise for nitco

        ErItTrdA = 703, ///item wise trading a/c
        ErAddLis = 704, ///address list with bal.
        ErCapiAc = 705, ///capital a/c

        //for multx
        ErGiftRe = 706, ///payout wise gift details
        ErTDSErr = 707, ///errors if any in tds list

        //for pax
        ErRG1stm = 708, ///rg I statement
        ErForm4R = 709, ///form 4

        //for soap
        ErMergDa = 710, ///merge data

        //for ElecX
        ErLotNoW = 711, ///lot no. wise register
        ErSalExW = 712, ///sales executive wise

        //for PantX
        ErStDlRp = 713, ///stock daily report

        //for plusx
        ErTreeDi = 714, ///directs tree

        ErBackDa = 715, ///backup data

        //for PantX
        ErRetGRg = 716, ///return goods reg.

        //for TradX
        EITPOR = 717, ///item wise purchase order report
        ErPurOSc = 718, ///purchase order scheme

        //for StudX
        ErStudLg = 719, ///for student ledger
        ErStuSum = 720, ///for student summary

        //for DepoX
        ErPackDu = 721, ///for package due

        //for Malani
        ErCformL = 722, ///for c-form seller list
        ErPairCl = 723, ///for pair calculation

        //for CottX
        ErFarmDu = 724, ///for farmer dues

        //for TradX
        ErStocSt = 725, ///for stock status

        //for BRKpX
        ErPpiDue = 726, ///for purchase pass invoice

        //for DepoX
        ErDailyP = 727, ///for daily payout option
        ErPyPair = 728, ///for payout pair details

        ErSbiDue = 729, ///for sales pass invoice
        ERsalAmD = 730, ///for sale amount bal.
        ERpurAmD = 731, ///for purchase amount bal.

        //for VatsX
        ErClWStr = 732, ///for class wise strength
        ErGdrWSt = 733, ///for gender wise strength

        //for AITA
        ErWhtStm = 734, ///for white/billed

        //for ALL
        ErTDSdue = 735, ///for tds dues

        //for VATSx
        ErFeeRcP = 736, ///for fee receipt printing

        //for NetX
        ErClubTr = 737, ///for club tree

        ErItPrfD = 738, ///for item profit in detail

        //for NetX
        ErClubMP = 739, ///for club matrix payout

        //for TraderAc
        ErDSlPCm = 740, ///for date wise sales/purchase comparison

        //for ClotX
        ErBilDet = 741, ///for bill detail

        //for VatsX
        ErFDuLst = 742, ///for fee due register
        ErFeePMS = 743, ///for fee paid monthly student wise
        ErFeePmc = 744, /// for fee paid monthly class wise
        ErCautFee = 745, /// for Caution money detail
        ErRecYSD = 746, /// for receipt yearly summary date wise
        ErRcNPay = 747, ///////////////////////////receipt and payment 24/09/10
        ErRentldg = 748,  //rent customer ledger
        ErSamledg = 749,  //FOR SAMPATII LEDGER
        ErSamsum = 750,    //for sampatti summary
        ErRentsum = 751,    //For Rent sumary
        ErRegistar = 752,   //for rent registar

        ////
        ////for Convx//anubha 04-06-09
        ErBillL = 801,
        ErCompD = 802, ///////////////////////anubha 13-06-09
        ErDrivD = 803,

        //for winex
        ErTrnSum = 805, ///madhvi 02-07-09
        ErTrpSum = 807,

        //madhvi 8-07-09 for tax on item
        Ertaxitm = 809,
        Ergrdisc = 810,
        Erqtydis = 811,

        ////////***************************anubha 22-07-09 For compx
        ErLnRate = 812, ///////////for landing rate  report
        ////////
        Ershehru = 813, ///-------for wajebaat
        ErWajeEn = 814,
        ErCustde = 815,
        //------------------------anubha 23-10-09 for PDC Option
        ErPDCReg = 816,
        ErUsAuthLi = 817, ///ErAuthRg//--- //////////////////////////anubha 12-11-2009 for user authorization list
        ErCustList = 818, /////////////////////////// shashwati 28-10-09 for Customer Information List in AutoX
        ErDlyJobSt = 819, ///////////////////// shashwati 01-11-09 for Daily Job Sheet
        ErToolList = 820,  //////////////////// shashwati 16-11-09 for Tools List
        //////////////////////////////////////////
        ErStcInR = 821, /////////////////////////// Shashwati 17-11-09
        ErMOP = 828, /////////// M.O.P report in Autox shashwati 29-01-2010
        ErJobCDet = 829, /////////////////// Job Card Detail report

        //  //02-12-09
        //  ErRcNPay = 822, ///////////////////////////receipt and payment
        //
        ErStcOut = 823, /////////////////// Material Outward Reg.
        EDlyTIss = 824, /////////////// Daily Tool Issue Register
        ErCashCus = 825, /////////////// Cash CustLst
        ErDlySlB = 826, /////////////// Daily Sales Book
        ErHoldRp = 827,
        ErContST = 828,
        ErRealTime = 829,
        ErOPStt = 830,
        ErMisCh = 831,
        ErPartsum = 832,
        ErMemPyOt = 833, ///////////////// member wise payout //////shashwati 03-03-2010
        ErPartPL = 834,
        ERatDiff = 835,
        ErShareST = 836,

        ErPayoutD = 837, /////////////////////////anubha 05-04-10
        ErDanikRg = 838, ///////////////////////////////////// Danik AAwak Register for Mohankheda
        ErDonatS = 839, /////////////////////////////////////// Donation Searching in Mohankheda
        ErPrtyMrnL = 840, ///////////////////////////////////////// Party wise MRN list
        ErMINL = 841, ///////////////////////////////////////////////////////Department wise Material Issue List
        ErPFSum = 842, ///////////////////////////// shashwati PF summary report



        ////
        ERDUEL = 1001,
        ERLOAN = 1002, ///LOAN LIST
        ERMEMB = 1003, ///MEMBERS LIST
        ERCHNO = 1004, ///Challan wise Report
        ERNEWB = 1005, ///BALANCE SHEET
        ERACCF = 1006, ///ACCOUNT FORM PRINT FOR SPECIFIC ENTRIES
        ERFUNL = 1007, ///Fund Due List
        ERINTC = 1008, ///Int. Check List

        //comes in use in dc 20-01-03
        ERMCBL = 2002, ///Multiple Bank
        ERFPDL = 2003, ///Fee Paid Book
        ERCOLL = 2004, ///Collection Book
        ERAPBi = 2005, ///for agent pay bill
        ERWHTT = 2006, ///for whole time table
        ERFPLI = 2007, ///for fees paid list

        //comes in use in ac 31-01-03
        ERSLHD = 3001, ///Salary Head Wise List
        ERVVSD = 3002, ///vvs deduction
        ERSLPL = 3003, ///Salary Paid List
        ERSCHA = 3004, ///Schedule - A
        ERPRFR = 3005, ///P.F. REPORT
        ERHRWC = 3107, ///House Rent & Water Charge
        ERESTP = 3108, ///Establishment Pay Bill
        ERPAYD = 3109, ///Pay Data Year Wise
        ERPOBI = 3110, ///Pay Order Bill
        Erscis = 3111, ///Form - A
        ERESIR = 3112, ///ESI REPORT

        //in aitac 17-02-03
        ERGRCH = 4001,
        //in crashac 28-02
        ERMEMO = 4201, ///Memory
        ERSUMM = 4202, ///For summary of accounts as per date or opposite a/c
        ERDRSD = 4203, ///For Driver List Side Wise
        ERSDDR = 4204,
        ERLRRE = 4205, ///Lorry Repairing
        ERDILI = 4206, ///Diesel List
        ERAVER = 4207, ///Average Of Lorry and Others

        //in babaac 28-02
        ERUSWL = 4401, ///For User Work List
        ERINCL = 4402, ///For Interest Calculation Report and a/c statement

        //in Kishan Ac
        ERSTRG = 4501,
        ERCALE = 4502, ///City and Area Wise Ledger
        ERUNGT = 4503, ///Used and Unused Gatthan Report

        //in DC
        ERFO16 = 4504,

        //in Capital
        EREXCH = 4505, ///Expenses Challan

        //in A/c and Inven
        ERUNBL = 4506, ///Packing Unit wise Balance
        EFFLOW = 4507, ///Funds Flow

        //in HeeraAc
        ERWAGL = 4508, ///Wages List
        ERSALI = 4509, ///Sales List
        ERPULI = 4510, ///Purchase List

        //in AC
        EREXPS = 4511, ///Classified statement
        ERNOMI = 4512, ///Nominal

        //in H, M, K
        ERAREG = 4513, ///A/c Register

        //in AC
        ERFuAl = 4514, ///Requirement of Funds under Allowances
        ERFuPa = 4514, ///Requirement of Funds under Pay
        ERCoSh = 4515, ///Compilation Sheet
        ERForC = 4516, ///FORM - C (SCIS)

        //in Kishan
        ERINLI = 4517, ///Interest List sales and purchase

        //in MK
        ERPLOS = 4518, ///Pledge O/s

        //in MP
        ERCFPR = 4519, ///C-Form Preview
        EBTDET = 4520, ///Builty Detail

        //
        EINALL = 4521, ///All Kind of Dr. and Cr.

        //in WFPL
        ERMeLe = 5001, ///Member Ledger
        EROYAL = 5002, ///Member Royalty List

        //in Heera
        ESALEB = 5501, ///Sales Bill

        //in Cos
        ERDUEcos = 6001,
        ErFrSch = 6002,
        ErSubAsn = 6003,

        //student Tc
        ERStudTC = 6004,
        ERResult = 6005,
        ERTc = 6006,
        ErBookAll = 6007,
        //14/07
        ErBookAllS = 6011,
        ErOutBooks = 6012,
        ErOutBook = 6008, ///for out book list
        ErCardLib = 6009, ///for card library
        ErCardSt = 6010, ///for staff library card
        Erbilist = 812, ///madhvi
        Erpaylst = 813,
        Erconlst = 814,
        Erduelst = 815, ///6-08-09
        Erpyregi = 816, ///1-10-09


        //kukshi
        //» ºnILESH22sep09 for kukshi voucher reports
        ErJornVhr = 817, ///journal voucher
        ErLedVhr = 818, ///legder
        ErChqRcpt = 819, ///chque receipt
        ErRjtChq = 820, ///rejected chque
        ErDmdInfo = 821, ///demand info

        //\\12Dec
        ErDayBk = 822, ///kukshi daybook
        //<<<<<<<<<<<<<<<<<<<<<<
        ErPdulst = 823,   //12-2-10
        Erprplst = 824,  //26-2-10
        ErDlyCol = 825, ///16/07/2010 shashwati for Daily Collection Report
        ErCashJr = 843, /////////////////////////////////////////////////////anubha 16/03/2010
        ErCredJr = 844, /////////////////////////////////////////////////////anubha 17/03/2010
        Erbankbk = 845,                           //madhvi 23-3-10
        ErPtaxdm = 846,
        ErPtaxWrd = 847, ///////////////////////////// ward wise property tax report shashwati 27-05-2010
        ErChqissd = 848, ///cheque issued
        Erbatchrp = 849,       //for batch report
        ErRashlst = 850,   //for rashan card list 17-11-2010
        ErdailyCl = 851,  //for daily collection report
        Erbudget = 852,    //for budget report
        ErRtduelst = 853, /// for rent duelist 24-05-2011
        Erincomst = 854,   //for income status report
        ErPartleg = 855,    //for Party/Supplier ledger of studdet type(69)
        ErPartysm = 856,   //for Party/Supplier summary
        Ertdsreg = 857,    //for TDS registar report
        ErRentlst = 858,   //for rent lst report //19-09-2011
        ErwatCTlt = 859,   //for water connection transfer list
        ErznwdSum = 860,   //for zone ward colny property summary report 11-10-2011
        ErzwcSum = 861,   //for zone ward colny rent summary report 11-10-2011
        ErzwcWSum = 862,   //for zone ward colny Water summary report 11-10-2011
        ErinveReg = 863,  //for inventory register 4-11-2011
        ErSumAcct = 864,  //for zone ward <acctwise summary of property //18-02-2012
        Ercashier = 865,  // for cashier cash book report.[ not in use]
        ErTrnslst = 866,  //for property transfer list report
        ErAnudan = 867,   //for anudan patrak report
        Erbankdet = 868, /// for bank detail report //26-07-2012
        ErkarmAgrim = 869, ///20-08-2012
        ErPrtaxlst = 870, ///24-08-2012 for property tax list of customers
        Erchqctrl = 871, ///for Cheque book control register //30-08-2012
        ErAsstrp = 872, /// for assets report
        ErEmdReg = 873, /// for emd register 12-10-2012
        ErNwduelst = 874, ///26-12-2012 for duelist new report in water
        ErAvakJavak = 875, ///9.4.2013 Aavak Javak Register
        ErEmpSumm = 876, ///03-04-2014
        ErEmpList = 877, ///06-05-2014
        ErDemVsColl = 878, ///25-05-2014

    }

    public ConstVariable()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public const int TypeCode_ETyCCityRe = 0;
    public const int TypeCode_ETyCFranch = 1;
    public const int TypeCode_ETyCFrCity = 2;
    public const int TypeCode_EAccount = 0;
    public const int TypeCode_EAgent = 1;
    public const int TypeCode_ELorD = 2;
    public const int TypeCode_EReceipt = 3;
    public const int TypeCode_ECompany = 4;
    public const int TypeCode_EPayment = 5;
    public const int TypeCode_EJournal = 6;//aakash
    public const int TypeCode_EColorSc = 7;
    public const int TypeCode_EPrintSc = 8;//It is of no use 28-02-03
    public const int TypeCode_EAcGroup = 9;
    //MB
    public const int TypeCode_ERepoMst = 10;
    public const int TypeCode_EInteMst = 11;
    //
    public const int TypeCode_EEmplPost = 11;
    public const int TypeCode_EEmplDesg = 12;
    public const int TypeCode_EEmplCCLa = 13;
    public const int TypeCode_EEmplVVHT = 14;
    //
    public const int TypeCode_ECityMst = 15;
    public const int TypeCode_ESDetMst = 16; //For House Category and Class
    public const int TypeCode_EFeePlan = 17; //For Student Table
    public const int TypeCode_EChallan = 18;
    public const int TypeCode_EStudent = 19;
    public const int TypeCode_EEmploye = 20;
    public const int TypeCode_ESalaryP = 21;
    public const int TypeCode_ESalaryV = 22;
    public const int TypeCode_EAdvSala = 23;
    public const int TypeCode_EUserLog = 24;
    //in CAP
    public const int TypeCode_EGrChMst = 11;
    public const int TypeCode_EGrChTrn = 12;
    public const int TypeCode_ECustMst = 13;
    public const int TypeCode_EDrivMst = 14;
    public const int TypeCode_EColoMst = 16;
    public const int TypeCode_EExpeDrv = 17;
    public const int TypeCode_EBillGrC = 18;
    public const int TypeCode_EDiChart = 19;
    public const int TypeCode_EExChart = 20;
    public const int TypeCode_EChalAc = 21;
    //
    public const int TypeCode_EPricMst = 26;
    public const int TypeCode_EPriPurc = 27;
    public const int TypeCode_EPriFood = 29;//For brk food plan
    //
    public const int TypeCode_ECateMst = 41;
    public const int TypeCode_ESalesOE = 42;//Sales
    public const int TypeCode_EPurchas = 43;//Purchase
    public const int TypeCode_EDiTraOE = 44;
    //in Crash
    public const int TypeCode_EStock = 10;
    public const int TypeCode_ECMLL = 11;
    public const int TypeCode_EDPPP = 12;
    public const int TypeCode_EUnitDet = 13;
    public const int TypeCode_EItemCat = 14;
    public const int TypeCode_EItemNam = 15;//i think it is in crash 28-03-02
    public const int TypeCode_EItemQlt = 16;
    public const int TypeCode_EEntries = 17;
    public const int TypeCode_EHead = 18;
    public const int TypeCode_EAtNm = 20;

    public const int TypeCode_Collector = 28;  // Added By Ajay

    //
    public const int TypeCode_EMemoran = 51;
    public const int TypeCode_EPettyCa = 52;
    //
    public const int TypeCode_EBillSal = 53; //For AC details of different kinds of Bill in => PO Bill
    public const int TypeCode_EPayOrde = 54; //For Pay Order
    public const int TypeCode_EPoBankH = 55;//different heads against whom cheque need to issued
    public const int TypeCode_EBudgetE = 56;
    //
    public const int TypeCode_EGodownM = 57;//For Kishan and rest
    public const int TypeCode_EUsGtDet = 58;//For Kishan used Gathan Entry
    //
    public const int TypeCode_EOpenVou = 59;//For Opening Voucher
    public const int TypeCode_EAreaMst = 60;
    //Shree Medicose
    public const int TypeCode_EManufac = 61;
    public const int TypeCode_EPatient = 62;//in SK for scheme
    public const int TypeCode_EDoctors = 63;
    //Form 16
    public const int TypeCode_EFormN16 = 64;
    //Heeranand
    public const int TypeCode_EStockTr = 65;//For Stock Transfer
    public const int TypeCode_EPendigP = 66;//For adjustment of purchase to godowns
    public const int TypeCode_EStateMs = 67;//State Master
    public const int TypeCode_ECustome = 68;//Customer Master
    public const int TypeCode_Ecuststor = 69;//customer store bill//madhvi
    //Heeranand
    public const int TypeCode_EPlacesM = 70;//For Places Master
    public const int TypeCode_ECartMst = 71;//For Cartage Master
    public const int TypeCode_ECommMst = 72;//For Commission Master
    public const int TypeCode_EDistMst = 73;//For Distance Master
    //Malani
    public const int TypeCode_ECostMst = 74;//For Costing Master
    public const int TypeCode_EProdTRN = 75;//For Production
    //Heeranand
    public const int TypeCode_EStockBi = 76;//For Stock Bifercation
    public const int TypeCode_EExpemst = 77;//For Expense Rate List
    public const int TypeCode_ETranspo = 78;//For Transporter
    //Malani
    public const int TypeCode_EOrdPurc = 79;//For Purchase Order
    public const int TypeCode_EOrdSale = 80;//For Sales Order

    //
    public const int TypeCode_ELabIssu = 83;//also used in hs for production
    public const int TypeCode_ELabRecd = 84;
    public const int TypeCode_ECreaTag = 85;
    //
    public const int TypeCode_EBhadaMs = 86;//Transport Bhada
    //
    public const int TypeCode_ERepoDes = 87;//Report Designer
    //
    public const int TypeCode_EModemst = 88;//Model
    public const int TypeCode_EDiesMst = 22;//Diesel
    //
    public const int TypeCode_EExpStat = 90;
    //
    public const int TypeCode_EPostMst = 91;//Post Setup
    //
    //92-95
    public const int TypeCode_ESelfPledge = 92;
    public const int TypeCode_ESelfDeposit = 93;
    public const int TypeCode_ECustPledge = 94;
    public const int TypeCode_ECustDeposit = 95;
    //
    public const int TypeCode_ESalseForAppro = 96;
    public const int TypeCode_ETagReport = 97;
    public const int TypeCode_ESaleApproReport = 98;
    public const int TypeCode_EApproRecived = 99;
    public const int TypeCode_EStockBifercateRep = 100;
    public const int TypeCode_ERemaking = 101;
    public const int TypeCode_ERemakingReturn = 102;
    public const int TypeCode_EEstimate = 105;
    //
    public const int TypeCode_EBilttyDet = 103;//by nutan For biltty Detail Form to Change Transport Detail Form name: bilttydet
    public const int TypeCode_ECFrmPreview = 104;//by nutan For Creat Cform. Form name: cfrmpreview
    //in Fair Wings
    public const int TypeCode_ERegMemb = 106;
    public const int TypeCode_EIncPrem = 107;
    public const int TypeCode_EContraV = 108;//aakash
    //in Baba Ac
    public const int TypeCode_ELordTyp = 109;
    //in Cosmos
    public const int TypeCode_EStFrCom = 110;//Stock From CompanyEStToCom = 111//Stock Return To Company
    public const int TypeCode_EStToCom = 111;
    //in Bajaj
    public const int TypeCode_EPrListT = 112;//Price List Type
    public const int TypeCode_EOpenSto = 113;
    //in Fair Wings
    public const int TypeCode_EIncRoya = 114;//Royalty Incentive
    public const int TypeCode_EIncProf = 115;//Profit  Incentive
    //in Bajaj
    public const int TypeCode_EGodBife = 116;
    public const int TypeCode_ECustCat = 117;//Category for customers
    public const int TypeCode_EGoldBlL = 118;//Gold / Black Listed
    public const int TypeCode_ECusCast = 119;//Cast of Customer
    //in Malani
    public const int TypeCode_ELetterW = 120;//Letter Writing
    //in Reminder
    public const int TypeCode_ERemindE = 121;
    //in Bajaj
    public const int TypeCode_EGroupBi = 122;
    public const int TypeCode_EItPacki = 123;//Item Packings
    //in Reminder
    public const int TypeCode_ERemComp = 124;
    public const int TypeCode_ERemCont = 125;
    public const int TypeCode_ERemExIn = 126;
    //in Bajaj;
    public const int TypeCode_EStudEmp = 127;
    public const int TypeCode_EPurChit = 128;
    public const int TypeCode_ESupAllo = 129;//Allotment of Items from Supplier
    //in Mahaveer  ;
    public const int TypeCode_ECoupCat = 130;//Coupon Category
    public const int TypeCode_ECoupAll = 131;//Coupon Allotment
    public const int TypeCode_ESalesMa = 132;//Salesman
    public const int TypeCode_ECoupCas = 133;//Coupon Cashed
    public const int TypeCode_EDiscMst = 134;//Discount Scheme
    //in S.Kumar   ;
    public const int TypeCode_EWorkerN = 135;
    //in Bajaj
    public const int TypeCode_ENegStoc = 136;//Negative Stock Adjustment
    //in Mahaveer  ;
    public const int TypeCode_EFinStoc = 137;//Finished Goods Entry
    //in Reminder  ;
    public const int TypeCode_ERemBlGr = 138;
    public const int TypeCode_ERemInit = 139;
    public const int TypeCode_ERemNota = 140;
    //in Tours     ;
    public const int TypeCode_EToAccVo = 141;
    //EToInvVo = 144
    //EToHotBi = 143//Hotel Bills
    public const int TypeCode_EToTraVo = 142;//Travelling Voucher
    //
    //in Bajaj
    public const int TypeCode_EPhyStoc = 145;//Physical Stock
    //in Perfume
    public const int TypeCode_EIteRela = 146;
    public const int TypeCode_EUniRela = 147;
    //in Agritech  ;
    public const int TypeCode_EOwnerMs = 148;//owner in AG and godowntype in SK
    //EagPlanR = 14;9//Agritech Plan Register
    //EagProcT = 14;9//Processing
    public const int TypeCode_EagSampI = 149;
    public const int TypeCode_EagCertT = 150;
    public const int TypeCode_EagSTReI = 151;
    public const int TypeCode_EagPkQty = 152;
    //in Bajaj     ;
    public const int TypeCode_EbjBlocT = 153;
    public const int TypeCode_EbjNGExI = 154;//None Group Extra Info
    //in Agritech  ;
    public const int TypeCode_EagItUnR = 155;//Relation Item With Unit
    //in Bajaj & Jugal
    public const int TypeCode_EbjDiscL = 156;
    //in HS        ;
    //in Tours     ;
    public const int TypeCode_EToPlanT = 158;
    public const int TypeCode_EToRoomT = 159;
    public const int TypeCode_EToHoteP = 160;//Hotel Plan
    //in HS        ;

    //in PF
    public const int TypeCode_EpfPackV = 162;//Paper Packing Voucher
    //in TA
    public const int TypeCode_EtaBankN = 163;//Bank Detail
    //in PF for biw
    public const int TypeCode_EpfAddOL = 164;
    //in Transport ;
    public const int TypeCode_EtrDespE = 165;
    public const int TypeCode_EtrArriE = 166;
    public const int TypeCode_EtrGodoT = 167;
    public const int TypeCode_EtrBookE = 168;
    public const int TypeCode_EtrDeliE = 169;
    //in Reminder  ;
    public const int TypeCode_EreLastN = 170;
    public const int TypeCode_EreMiddN = 171;
    //in MM        ;
    public const int TypeCode_EmmReplC = 172;//Replacement Challan
    //in AG
    public const int TypeCode_EagPurcE = 173;//of use for purchase estimate
    public const int TypeCode_EagBookC = 174;//Booking Challan
    //in MM         ;
    public const int TypeCode_EmmCoupI = 175;//Voucher For Incoming of Coupons
    public const int TypeCode_EmmCoupP = 176;//Coupon Name and their Points
    //in PF         ;
    public const int TypeCode_EpfScheN = 178;
    //in AG        ;
    public const int TypeCode_EagLotNo = 179;
    //in HR
    public const int TypeCode_EhrWType = 180;
    public const int TypeCode_EhrWardD = 181;
    public const int TypeCode_EhrOPDRe = 182;
    public const int TypeCode_EhrIPDCa = 183;
    public const int TypeCode_EhrXRayR = 184;
    public const int TypeCode_EhrPathR = 185;
    public const int TypeCode_EhrBillP = 186;
    public const int TypeCode_EhrReguE = 187;//Regular Visits Detail of Patient Admitted
    //in SK for Khanna
    public const int TypeCode_EskCommL = 189;
    public const int TypeCode_ETaxeMst = 190;
    //in AP        ;
    public const int TypeCode_EapDiscS = 191;//Discount and Cost price setup
    //in SK for Cot;ton
    public const int TypeCode_EskSaleP = 192;//Sale Patti
    //in SK for Cotton
    public const int TypeCode_ELocaMst = 193;//Location Master
    public const int TypeCode_EInsuEnt = 194;//Insurance Entry
    //For Bal. Tr;.
    public const int TypeCode_EBalTran = 195;
    //in SK for Cot;;ton
    public const int TypeCode_ESpareIn = 196;
    public const int TypeCode_ESparOut = 197;
    public const int TypeCode_ESpInOrd = 198;
    public const int TypeCode_ESpOutOr = 199;
    public const int TypeCode_ESparRet = 200;
    //in SK for tra;nsport
    public const int TypeCode_ETruckNo = 201;
    public const int TypeCode_EDriverN = 202;
    //in SK for inv;;estor/ 7-10-05 NOW ALSO FOR FINANCE WHEN VEH. SELLER DIRECTLY GETS CREDITED
    public const int TypeCode_EMoneyIn = 203;
    //in SK for spinning
    public const int TypeCode_ESpinnPr = 204;
    public const int TypeCode_ESpiCost = 205;
    //in SK for investor
    public const int TypeCode_ENAVmast = 206;
    //in SK for con;structor
    public const int TypeCode_EToolsMa = 207;
    public const int TypeCode_EBranchM = 208;
    public const int TypeCode_EProjMst = 209;
    public const int TypeCode_ESiteMst = 210;
    //in SK for investor
    public const int TypeCode_EDIVmast = 211;
    //in SK for con;structor
    public const int TypeCode_EDVCentr = 212;//daily vehicle card
    public const int TypeCode_EconMeSh = 213;//mesaurement sheet
    public const int TypeCode_EconAofB = 214;//abstract of bill
    public const int TypeCode_EconStoS = 215;//site to site transfer
    public const int TypeCode_EconWbyV = 216;//work done by vehicle entry
    public const int TypeCode_EconTrBh = 217;//transporter bhada agreement
    public const int TypeCode_EconHiCh = 218;//hired vehicle charges
    public const int TypeCode_EconLabS = 219;//labour slip
    public const int TypeCode_EconWord = 220;//work order entry
    //             ;
    //in SK for reportAS
    public const int TypeCode_EreFixCo = 221;
    public const int TypeCode_EreDMhrV = 222;//this is for daily or other options
    public const int TypeCode_EreProdU = 223;//this is for production unit name
    //in SK for constructor
    public const int TypeCode_EconExpB = 224;//expenses bills
    public const int TypeCode_EconRepV = 225;//repairing voucher
    //in SK for proformaAc
    public const int TypeCode_EpayRemL = 226;//payment reminder letter
    public const int TypeCode_ENarrMst = 227;//narration
    //
    public const int TypeCode_EmmPrepC = 228;//purchase replacement challan
    //
    public const int TypeCode_EletHead = 229;//letter head
    public const int TypeCode_ErepRemL = 230;//repairing reminder
    //
    public const int TypeCode_EhsCommM = 231;//new comm. setup
    public const int TypeCode_EhsPuCom = 232;//new purchase comm. setup
    public const int TypeCode_EhsLaboV = 233;//labour voucher
    public const int TypeCode_EhsLaboS = 234;//labour setup
    public const int TypeCode_ELabourM = 235;
    //For SUR
    public const int TypeCode_EStagTyp = 236;//stages master
    public const int TypeCode_EProdReq = 237;//prod. requirement
    //For REP//saklecha ji
    public const int TypeCode_EreGrosP = 238;//gross profit chart
    public const int TypeCode_EreNetPr = 239;//net profit chart
    public const int TypeCode_Eballia = 239;//balance sheet for liability
    public const int TypeCode_Ebalass = 240;//balance sheet for asset
    public const int TypeCode_Ecashflw = 241;//cash flow
    public const int TypeCode_Eprjcost = 242;//Cost of Project
    public const int TypeCode_Erplan = 243;//New Plan
    public const int TypeCode_EOtherReq = 244;//steam/water/power
    public const int TypeCode_EClStkReq = 245;//Closing Stk requirement
    public const int TypeCode_Ereject = 246;//rejetion
    //---------------
    //For Mahavir
    public const int TypeCode_EmmReplR = 240;//return by customer
    public const int TypeCode_EmmRepPR = 241;//return to company
    //For Hocker
    public const int TypeCode_EBrandPrL = 243;//brand wise price list
    public const int TypeCode_ERecWItem = 244;//receipt as per items
    //For Pharma
    public const int TypeCode_EphaRawIs = 245;//For raw material issue
    public const int TypeCode_EphaPackI = 246;//For pack material issue
    //For owner of machines
    public const int TypeCode_EMachOwne = 247;//For macine owner
    public const int TypeCode_EBlowingS = 248;//For blowing
    public const int TypeCode_ECandicRe = 249;//For candic
    public const int TypeCode_EDrawingR = 250;//For drawing
    //For Hocker
    public const int TypeCode_EDailyEnt = 251;//For daily entry
    //For Cotton
    public const int TypeCode_ESparOpSt = 252;
    public const int TypeCode_ESpInqEnt = 253;//inquiry entry for spare purchase
    //For Spinner
    public const int TypeCode_EMachServ = 254;//For machine servicing
    public const int TypeCode_ELaboCate = 255;//For labour category
    //For adat kharidi and bechwali
    public const int TypeCode_EcotAdatP = 256;//For adat purchase and sale
    public const int TypeCode_EcotElecV = 258;//For electricity consumption
    //---for student
    public const int TypeCode_ErMonths = 237;
    public const int TypeCode_EstuOccup = 238;
    public const int TypeCode_ErHouse = 239;
    public const int TypeCode_Erstud = 240;
    public const int TypeCode_ERnation = 241;
    public const int TypeCode_ERedu = 242;
    public const int TypeCode_ErMtongue = 243;
    public const int TypeCode_Ersession = 244;
    public const int TypeCode_Ercategory = 245;
    public const int TypeCode_Ercaste = 246;
    public const int TypeCode_ErBooks = 247;
    public const int TypeCode_ErSubject = 248;
    public const int TypeCode_EBus = 249;
    public const int TypeCode_ERroute = 250;
    public const int TypeCode_ERBookIsRe = 251;
    //14/07
    public const int TypeCode_ERBookStaf = 263;//Staff Issue Return
    public const int TypeCode_ErLibrary = 252;
    public const int TypeCode_ErClassTT = 253;
    public const int TypeCode_ErRptBookD = 254;
    public const int TypeCode_ErEmployee = 255;
    public const int TypeCode_EExamType = 256;
    public const int TypeCode_EmarkPlan = 257;
    public const int TypeCode_EEntrance = 258;
    public const int TypeCode_ETc = 259;
    public const int TypeCode_EGotra = 260;
    public const int TypeCode_ECompl = 261;
    public const int TypeCode_EWeight = 262;
    public const int TypeCode_EMangl = 263;
    public const int TypeCode_EincGr = 264;
    public const int TypeCode_EFMType = 265;
    public const int TypeCode_ERelUnc = 266;
    public const int TypeCode_ERelBro = 267;
    public const int TypeCode_ERelMam = 268;
    ////////anubha 18-12
    public const int TypeCode_ErBranch = 269;
    //----------15-06-05
    //we have taken this voucher separately becuase it will store date value
    //also along with other info. so instead of grid we have used entry system
    public const int TypeCode_EhsLabAdv = 301;//labour advance
    public const int TypeCode_EhslabPay = 302;//labour payment voucher
    public const int TypeCode_EhsLShift = 303;//labour shift
    //For school
    public const int TypeCode_EstPreDue = 304;//student previous due entry
    //For labour
    public const int TypeCode_EhsLOccAd = 305;//occassional advance to labour
    //For BABA
    public const int TypeCode_EFamiHead = 306;//Family Head
    public const int TypeCode_EInstDueE = 307;//Instalment Due Entry
    //
    public const int TypeCode_ECredNote = 308;
    public const int TypeCode_EDebiNote = 309;
    //For Fleet Owner
    public const int TypeCode_EDespChal = 310;
    public const int TypeCode_EBillingO = 311;
    public const int TypeCode_EChgsTran = 312;
    //For Lawyer;
    public const int TypeCode_ElawFeesR = 313;
    public const int TypeCode_ElawCaseH = 314;
    //For School;
    public const int TypeCode_EexamPlan = 315;
    public const int TypeCode_Equalific = 316;
    //For TraderX
    public const int TypeCode_EcurrRate = 317;
    //For Hospital
    public const int TypeCode_EadmNdisc = 318;
    //For convoy
    public const int TypeCode_EbodySetu = 319;
    //For opening bills
    public const int TypeCode_EopenBill = 320;//For opening entries with bills
    public const int TypeCode_EopenRece = 321;
    //For share tr.
    public const int TypeCode_EshareBil = 322;
    //For depos; cnfs
    public const int TypeCode_EtaxItemW = 323;// item wise tax fixation
    public const int TypeCode_EschemeSe = 324;// scheme setups
    public const int TypeCode_EgiftSetu = 325;// gift setups
    public const int TypeCode_EgiftRece = 326;// gift received
    public const int TypeCode_EgiftIssu = 327;// gift issue
    public const int TypeCode_EIncePaid = 328;// incentive paid
    //
    public const int TypeCode_EothScheS = 329;// other schemes setups like idea
    public const int TypeCode_EdepCredR = 330;// depo credit request
    //
    public const int TypeCode_EEstiRetu = 331;//estimate returned
    //For share traders
    public const int TypeCode_ESharExpS = 332;//share expenses setup
    //For depos; cnfs
    public const int TypeCode_ESampRece = 333;//sample received
    //For aparts
    public const int TypeCode_EaddPurcP = 334;//add in % in purchase
    //For VAT applicability
    public const int TypeCode_EvatOnAcc = 335;//to specify which a/c will have vat applicability or not
    //For depos
    public const int TypeCode_EDummyPrD = 336;//For joining with dummy product and its delivery
    public const int TypeCode_EbillSchE = 337;//For schemes which applies in mib sale for schemed items e.g. sugar
    //29/08
    public const int TypeCode_EProdInfo = 338;
    //For netwx   ;
    public const int TypeCode_EPayDueVE = 339;//pay out due entry
    public const int TypeCode_EPayPaidE = 340;//pay out paid entry
    //For synox   ;
    public const int TypeCode_ESaleExtV = 341;//sales extra voucher
    //For mallx
    public const int TypeCode_EGeBarCod = 342;//generate bar code
    //For netwx   ;
    public const int TypeCode_EGeIncPay = 350;//generate pay out
    public const int TypeCode_EGeIdCode = 351;//generate id codes
    public const int TypeCode_EIdSoldEn = 352;//sold id nos.
    public const int TypeCode_EBVentryT = 352;
    public const int TypeCode_EFundMast = 353;//various funds head
    //For depox   ;
    public const int TypeCode_EJoinType = 354;
    public const int TypeCode_EJoExcelS = 355;
    public const int TypeCode_EDisCouEn = 356;
    public const int TypeCode_EDebiChar = 357;//charges debit to any body
    public const int TypeCode_EPayRPack = 358;//payment received for package
    //For netwx   ;
    public const int TypeCode_EMIBDueVE = 359;//mib closing voucher
    public const int TypeCode_EFrnDueVE = 360;//franchisee closing voucher
    //For depox   ;
    public const int TypeCode_EPRPakDep = 361;//payment received from depo for package
    public const int TypeCode_EFundTran = 362;//fund transfer entry
    public const int TypeCode_EGiftBook = 363;//gift booking
    //For Product ;Transfer
    public const int TypeCode_EFGTranEn = 364;//finished goods transfer
    public const int TypeCode_ERepairVe = 365;//repairing voucher entry
    public const int TypeCode_EStampEnt = 366;//stamping entry
    //For PAX
    public const int TypeCode_EexcOnAcc = 367;//excise on which kind of a/c//s
    public const int TypeCode_EqtyOnDis = 368;//qty. discount
    public const int TypeCode_EQCheckSc = 369;//quality checking scheme
    public const int TypeCode_EQCEntryV = 370;//quality checking entry
    public const int TypeCode_ETestMstV = 371;//test master
    public const int TypeCode_EParamMst = 372;//parameters master
    public const int TypeCode_EexcSetAc = 373;//excise/expenses setup
    //For CollX   ;
    public const int TypeCode_EscholarT = 374;//scholarship type
    public const int TypeCode_EMainSubj = 375;//main subject  name
    //For MallX   ;
    public const int TypeCode_ECustTypW = 376;//For customer type list
    //For CollX
    public const int TypeCode_EAdmFormE = 377;//For adm. form rec. amt.
    //For MallX
    public const int TypeCode_EBillEntV = 378;//general bill entry for bar coded companies
    //For COTTX   ;
    public const int TypeCode_ETradAcTy = 379;//trading account type
    //For CollX   ;
    public const int TypeCode_EBookPurc = 380;//books purchase entry
    public const int TypeCode_EBookCate = 381;//books category
    //For ExpoX
    public const int TypeCode_ESchedMst = 382;//schedule master
    //For Tradx   ;
    public const int TypeCode_ECountMst = 383;//counter master
    //For ExpoX   ;
    public const int TypeCode_EPuConfEn = 384;//purchase confirmation entry
    public const int TypeCode_EExcOpEnt = 385;//excise opening entry
    public const int TypeCode_EVATtranE = 386;//For vat transfer entry
    //For MallX
    public const int TypeCode_EBilCTypW = 387;//For BILL customer type list
    //For ElecX
    public const int TypeCode_EStaffAtt = 388;//For staff attendance
    //For CottX
    public const int TypeCode_EPayGeneC = 389;//For payment made to general customers multiple
    public const int TypeCode_ERecGeneC = 390;//For receipt made from general customers multiple
    //For PantX
    public const int TypeCode_EPackSchE = 391;//For packing scheme entry
    public const int TypeCode_EProfItmW = 392;//For profit item wise
    //For ConvX
    public const int TypeCode_EModeCate = 393;//For model category
    public const int TypeCode_EaiTollSe = 394;//For toll tax setup
    //For FeeRet
    public const int TypeCode_EFeesRetu = 395;//For fee return
    //For ConsX
    public const int TypeCode_EMatIssuS = 396;//For material issue to site
    //For BRKpx
    public const int TypeCode_EEstiPurc = 397;//For purchase estimate INWard
    public const int TypeCode_EskPurcP = 398;//For purchase pass invoice
    public const int TypeCode_EsaleAdvi = 399;//For sales pass invoice
    // Ashish-03/19/08//  Reserv for me 401-420
    //-- Report Code
    //** used on//Labour Attendence report// form
    public const int TypeCode_EMstMtReport = 394;//Material Report
    public const int TypeCode_EconRLab = 405;//Labour Report
    public const int TypeCode_EconRExp = 406;//Remain part of Expsheet Report
    public const int TypeCode_EMartSht = 409;//Current Stock Report
    public const int TypeCode_ESitCash = 410;//Site Cash
    public const int TypeCode_ESupCash = 411;//Supervisor Cash
    public const int TypeCode_ELabDet = 412;
    public const int TypeCode_ESupDet = 413;
    //***** Entry Code
    public const int TypeCode_EMaterialSheet = 390;//material sheet
    public const int TypeCode_EconIssu = 401;//Material Issue
    public const int TypeCode_EconRetu = 402;//Material Return
    public const int TypeCode_EconTran = 403;//Material Transfer
    public const int TypeCode_EconCosu = 404;//Material Consumpation
    public const int TypeCode_EConSupRec = 407;//Supvervisor Receive
    public const int TypeCode_EConSupexp = 408;//Supvervisor Expenses
    public const int TypeCode_ESupCashEnt = 409;//Supervisor Cash
    public const int TypeCode_EDrivCashEn = 410;//Driver Cash
    public const int TypeCode_ESiteDislEn = 411;//Diesel Entry
    //For VATsx     ;
    public const int TypeCode_EMediumSt = 421;//For medium
    //For Clotx     ;
    public const int TypeCode_EBrokEntV = 422;//For brokerage entry
    //For VATsx
    public const int TypeCode_EReliType = 423;//For religion type
    public const int TypeCode_ETDSmstRt = 424;//For tds master
    public const int TypeCode_ETDSdueEn = 425;//For tds due entry
    //For FinaX
    public const int TypeCode_ELoanPayV = 426;//For loan payment voucher
    public const int TypeCode_EInsCompM = 427;//For insurance company master
    public const int TypeCode_EExpAfAct = 428;//For expenses affect a/c
    public const int TypeCode_ESVchType = 429;//For sub vch type
    public const int TypeCode_ELoanCrDr = 430;//For loan cr/dr through single entry
    public const int TypeCode_ELoanRec = 431;//For dictionary of hindi
    //For proforac
    public const int TypeCode_EPOSORelE = 440;//For po/so relation
    public const int TypeCode_EFollowUp = 441;//For followup entry
    //**********momin
    public const int TypeCode_EDiscWord = 444;
    public const int TypeCode_EHinConv = 445;
    public const int TypeCode_EPenaAdj = 448;//entry
    //******Entry code for SAIL
    public const int TypeCode_EInward = 455;
    public const int TypeCode_EBookingSlip = 456;
    public const int TypeCode_EExpBookSlip = 457;
    public const int TypeCode_EFuelEntry = 458;
    public const int TypeCode_EVehExp = 459;
    //************* shashwati 15-11-08
    public const int TypeCode_ECardChit = 460;
    public const int TypeCode_EBillGen = 461;//shashwati 09/01/09
    //******************************************************anubha
    public const int TypeCode_EChanBilE = 480;//For challan/bill Entry
    public const int TypeCode_EMastTDS = 481;//For TDS Setup
    public const int TypeCode_ETDSPay = 482;//For tds payment
    public const int TypeCode_EProdTrg = 486;//executive target
    public const int TypeCode_EAreAollE = 487;//Area Allotement
    public const int TypeCode_ECDSchMst = 488;//For Scheme master
    //****for excise
    public const int TypeCode_EMachPurc = 491;
    public const int TypeCode_EGiftMst = 492;//chemx shashwati 15/01/09
    public const int TypeCode_EGiftCase = 493;//shashwati 17/01/09
    //******** madhvi 26-12-08
    public const int TypeCode_EProofset = 500;//For proofset
    //*******************anubha 14-01-09 for vastx
    public const int TypeCode_EFeCrtNot = 568;
    public const int TypeCode_EFeDbtNot = 569;
    public const int TypeCode_EEncloser = 570;
    public const int TypeCode_EGodPriL = 571;
    //***********anu;bha 10-02-09
    public const int TypeCode_ESaleOrde = 578;
    public const int TypeCode_EpropTax = 579;
    public const int TypeCode_EZoneMast = 580;//Shashwati 12-02-09
    public const int TypeCode_EWardMast = 581;//Shashwati 12-02-09
    public const int TypeCode_EColMast = 582;//Shashwati 12-02-09
    public const int TypeCode_EAnRevst = 583;//Madhvi
    public const int TypeCode_EColAlot = 584;//shashwati 12-02-09
    public const int TypeCode_ETaxset = 585;// 13-02-09-
    public const int TypeCode_EFlorMast = 586;//--------anubha 25-08-09
    public const int TypeCode_EDepamst = 587;//28-08-2010
    public const int TypeCode_ENiveshM = 588;//7/10/2010
    public const int TypeCode_EScemast = 589;//For scheme master for social security scheme
    //////////////////////////////stampx
    public const int TypeCode_EExeMast = 600;
    public const int TypeCode_Ewaterst = 601;//madhvi
    public const int TypeCode_EWatApp = 602;//madhvi
    public const int TypeCode_Ewatbill = 603;//For waterbill generation
    public const int TypeCode_EwtSerch = 604;
    public const int TypeCode_EWattax = 605;
    public const int TypeCode_EWpaymt = 612;//water Payment
    public const int TypeCode_EDischgs = 613;
    ////////MADHVI    ;
    public const int TypeCode_Emastnw = 614;
    public const int TypeCode_Emasted = 615;
    public const int TypeCode_Emastdl = 616;
    //madhvi      ;
    public const int TypeCode_Econser = 617;
    public const int TypeCode_Enwtpay = 618;
    public const int TypeCode_Ebookmst = 619;
    public const int TypeCode_EWopbal = 620;//19-08-09
    //ºnILESH02oct09 connection type setup
    public const int TypeCode_EConStup = 621;
    public const int TypeCode_EPrTxBll = 622;
    public const int TypeCode_EPrOpEnt = 623;
    //<<<<<<<<<
    public const int TypeCode_EFrgtPaid = 788;//For freight payment entry
    public const int TypeCode_EFrgtBill = 787;//For freight voucher entry
    //madhvi 27-1-10
    public const int TypeCode_ERentRec = 624;
    public const int TypeCode_EPconnset = 625;
    public const int TypeCode_EPDischg = 626;//For property late fee and discount setup
    public const int TypeCode_EPropPay = 627;
    public const int TypeCode_Ewatextr = 628;
    public const int TypeCode_Ekarylist = 629;//For kary samuho ki suchi in lekha s.s.
    public const int TypeCode_EConChrg = 630;//For connection charges
    public const int TypeCode_Ebuildreg = 631;//For building register
    public const int TypeCode_Esheritag = 632;//For statues and heritage
    public const int TypeCode_ERdstreet = 633;//For road and street register
    public const int TypeCode_EbridgeFl = 634;//For bridge ;flyover entry
    public const int TypeCode_EdrainsUn = 635;//For drains entry
    public const int TypeCode_EPubLight = 636;//For public light entry
    public const int TypeCode_Epondlake = 637;//For pond and lake
    public const int TypeCode_EplantMac = 638;//For plant and machinary
    public const int TypeCode_EvehicleE = 639;//For Vehicle entry
    public const int TypeCode_EoffEqumt = 640;//For office equipment
    public const int TypeCode_Efurniture = 641;//For furniture
    public const int TypeCode_EcompPeri = 642;//For computer & peripheral
    public const int TypeCode_Esoftent = 643;//For software entry
    public const int TypeCode_EInvestRg = 644;//For investment entry
    public const int TypeCode_Eloanregs = 645;//For loan register
    public const int TypeCode_EInvetory = 646;//For inventory entry
    public const int TypeCode_Ebhumireg = 647;//For bhumi register
    public const int TypeCode_EjalkaryP = 648;//For Jal kary prday
    public const int TypeCode_Eprgatikr = 649;//For pragati kary
    public const int TypeCode_Eanayupkr = 650;//For anay upkaran vivran
    public const int TypeCode_Ejivitscn = 651;//For jivit scandh entry
    public const int TypeCode_Enivshvi = 652;//For nivesh vivran
    public const int TypeCode_Enagadsh = 653;//For nagad shesh
    public const int TypeCode_Ebanksh = 654;//For bank shesh
    public const int TypeCode_EPrAgrim = 655;//For pradayak Agrim
    public const int TypeCode_Ekaragri = 656;//For karmchari agrim
    public const int TypeCode_Edatatrn = 657;//For data transfer
    public const int TypeCode_Ebatchen = 658;//For batch entry
    public const int TypeCode_Eshopmst = 659;//For shopmaster
    public const int TypeCode_Eshopall = 660;//For shopallotment
    public const int TypeCode_Eshopbill = 661;//For shopbill generation
    public const int TypeCode_Eshoppay = 662;//For shop bill payment
    public const int TypeCode_EOpshop = 663;//For shop opening entry
    public const int TypeCode_EWamtadj = 664;//For water tax amount adjustment
    public const int TypeCode_Ershcrd = 665;//For rashan card entry
    public const int TypeCode_Erelamst = 666;//For relationship master
    public const int TypeCode_Eplotmst = 667;//For plot master
    public const int TypeCode_Erentmst = 668;//For rent master
    public const int TypeCode_Erentrela = 669;//For Rent Account relation master
    public const int TypeCode_EWorkbill = 670;//For work bill form
    public const int TypeCode_Eotherbill = 671;//For other bill
    public const int TypeCode_Esocialsce = 672;//For social scheme entry
    public const int TypeCode_ECompmst = 590;////vivek
    public const int TypeCode_EPermissionmst = 591;////vivek
    public const int TypeCode_EBuildingcon = 592;////vivek
    public const int TypeCode_Edocumentmst = 593;////vivek
    public const int TypeCode_Ecomplaintreg = 673;//For complaintregister
    public const int TypeCode_ECertificate = 674;//For Certificate 
    public const int TypeCode_EBuildingper = 675;//For building permission

    public const int TypeCode_EWrelaopn = 830;//For water relationship setup for year end
    public const int TypeCode_ERrelaopn = 831;//For rent relationship setup for year end
    //EIMEstuddet
    public const int TypeCode_EIncMast = 1002;//// Identity master
    public const int TypeCode_ERecpentry = 1003;//// Recipt entry
    public const int TypeCode_EDemandT = 1004;////demand type 
    public const int TypeCode_EMatInRg = 809;//For material inward register
    public const int TypeCode_EEmpSto = 1005;//For persons in store
    public const int TypeCode_EIncDetail = 1006;//For income detail
    public const int TypeCode_EMenuEntry = 1007;//For menu entry(bhojan)
    public const int TypeCode_Ebranchset = 1008;   //For employee tree setup
    public const int TypeCode_EWtRelaMst = 1011;   //For water Relation

    public const int TypeCode_EBuildingType = 702;
    public const int Tax_Disc = 10;
    public const int TypeCode_Epropinc = 101;
    public const int TypeCode_Eshopinc = 102;
    public const int typeCode_ERNtLate = 678;
    public const int TypeCode_EPropBalTr = 901;
    public const int TypeCode_RentMaster = 902; //Rent Master 
    public const int TypeCode_ShopLocation = 903; //Location 
    public const int TypeCode_ShopDiviSion = 904; //Division 
    public const int TypeCode_RationCardType = 905;
    public const int GrouCode_EIncomeInd = 13;//Indirect Incomes
    public const int GroupCode_ECreditors = 22;// 
    public const int BasAcCode_EBasicSal = 213;//kya
    public const int TypeCode_Panjiyan = 1009;
    public const int Report_Saudareport = 1010;

    public const int Group_COTTON_BALES = 13;
    public const int Group_Cotton_Cake = 4;
    public const int Group_Cotton_SEEDS = 3;
    public const int Group_BlackSheed = 6;//aakash 02/13/2015

    public const int Group_OIL = 5;
    public const int Group_WASHOIL = 15;

    public const int TypeCode_DirectExpenses = 12;
    public const int TypeCode_DirectIncome = 11;
    public const int TypeCode_IndirectExpenses = 14;
    public const int TypeCode_IndirectIncome = 13;
    public const int TypeCode_Taxes = 37;
    public const string saudaReport = "2";
    public const string sauda_Statement = "1";
    public const string AckReport = "3";
    public const int SellPurchReport = 1;
    public const string Report_report = "4";
    public const string saudaReport_Cake = "10";
    public const string RTGSReport_Bank = "17";

    public const int TypeCode_SaleBill = 42;
    public const int TypeCode_PurchaseBill = 43;
    public const int TypeCode_CrNote = 44;//sourabh 16-nov-2015 for credit note
    public const int TypeCode_DrNote = 46;//sourabh 16-nov-2015 for debit note
    public const int TypeCode_Discount = 45;
    public const string TypeReport_DrNote = "5";
    public const string TypeReport_CrNote = "4";
    //public const int AckReport = 3;


    public const string TypeReport_SaudaSummary = "6";
    public const string TypeReport_PassingList = "7";
    public const string TypeReport_BillingList = "8";
    public const string TypeReport_Center = "9";
    public const string TypeReport_LotReg = "11";
    public const string TypeReport_TentativePayment = "12";
    public const string TypeReport_TentativeReceipt = "13";
    //Insurance Company
    public const int TypeReport_InsuranceComp = 901;
    //Claim
    public const int TypeReport_InsuranceClaim = 2897;
    // public const int TypeReport_InsuranceClaim= 906;
    //Verify
    public const int TypeReport_Insuranceverify = 907;
    //Goods Master
    public const int TypeReport_InsugdMst = 908;
    //Agent Master
    public const int TypeReport_MstAgent = 909;
    //Agent Master
    public const int TypeReport_DocMst = 910;


    public const int TypeReport_InsurStock = 15;

    public const string TypeReport_Aquittance = "30";

    public const string TypeReport_Bank = "31";
    public const string TypeReport_Summary = "32";

    public const string TypeReport_GPF = "33";
    public const string TypeReport_Slip = "34";
    public const string TypeReport_Nominal = "35";

    public const int TypeCode_AddreeProof = 902;

    public const int TypeCode_ESaleRet = 81;
    public const int TypeCode_EPurcRet = 82;

    public const int TypeCode_EhsStocI = 157;//Stock Adjustment // Stock Entry 
    public const int TypeCode_EhsStocO = 161;//Stock Out // Consumption

    public const int TypeCode_NewLot = 2866;//Create New Lot

    //public const int TypeCode_ESaleRet = 81; //Stock Entry 157 
    ////public const int TypeCode_EPurcRet = 82;  
    //public const int TypeCode_ConsumEnt = 82; //Consumption Entry 161
    public const string TypeCode_ClosingBank = "89";
    public const string TypeCode_ClosingBankLoan = "987";
	public const string TypeCode_StuddetTermnConditionType= "905";//170706
    public const string TypeCode_StuddetTermnCondition = "906";//170706
}
