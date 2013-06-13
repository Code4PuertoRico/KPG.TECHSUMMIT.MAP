using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPG.TECHSUMMIT.MAP
{
    public class OBJ_Establishment
    {
        public int estId { get; set; }
        public string estName { get; set; }
        public string estTrackingNumber { get; set; }
        public string estAuthNumber { get; set; }
        public string estStatusCode { get; set; }
        public string estStatus { get; set; }
        public string estBusinessType { get; set; }
        public DateTime estOpeningDate { get; set; }
        public string estLegalStructure { get; set; }
        public string estRegion { get; set; }
        public string estMunicipality { get; set; }
        public string estUnitNumber { get; set; }
        public bool estChain { get; set; }
        public bool estFranchise { get; set; }
        public string estPhone1 { get; set; }
        public int estPhoneType1 { get; set; }
        //public string estPhoneTypeName { get; set; }//NEW
        public string estPhone2 { get; set; }
        public int estPhoneType2 { get; set; }
        //public string estPhoneTypeName2 { get; set; }//NEW
        public string estEmail { get; set; }
        public string estAddPostal1 { get; set; }
        public string estAddPostal2 { get; set; }
        public string estAddPostalCity { get; set; }
        public string estAddPostalState { get; set; }
        public string estAddPostalZipCode { get; set; }
        public string estAddPhysical1 { get; set; }
        public string estAddPhysical2 { get; set; }
        public string estAddPhysicalNeighborhood { get; set; }
        public string estAddPhysicalStreet { get; set; }
        public string estAddPhysicalKilometer { get; set; }
        public string estAddPhysicalCity { get; set; }
        public string estAddPhysicalState { get; set; }
        public string estAddPhysicalZipCode { get; set; }
        public string estAddAdditionalInfo { get; set; }
        //new
        public string estLatLong { get; set; }
        public string estLatestMod { get; set; }
        //public OBJ_EstablishmentOtherData oEstablishmentOtherData { get; set; }
        //public OBJ_EstablishmentCategory[] oEstablishmentCategory { get; set; }
        //public OBJ_EstablishmentService[] oEstablishmentService { get; set; }
    }
}
