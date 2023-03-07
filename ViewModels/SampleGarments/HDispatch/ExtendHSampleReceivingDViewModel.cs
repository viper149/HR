namespace DenimERP.ViewModels.SampleGarments.HDispatch
{
    public class ExtendHSampleReceivingDViewModel
    {
        public int RCVDID { get; }
        public string CUST_NAME { get; }

        public ExtendHSampleReceivingDViewModel(int rcvdid, string custName)
        {
            RCVDID = rcvdid;
            CUST_NAME = custName;
        }
    }
}
