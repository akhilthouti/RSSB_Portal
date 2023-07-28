namespace INDO_FIN_NET.Models
{
    public class ClsJointAccountFinacle
    {
        
   
            public string Name { get; set; }
            public string Name1 { get; set; }
            public string Name2 { get; set; }
            public string Name3 { get; set; }
            public string CustomerNO { get; set; }
            public string CustomerNO1 { get; set; }
            public string CustomerNO2 { get; set; }
            public string AccountNo { get; set; }
            public string Branch { get; set; }
            //public string ErrorMessage { get; set; }

            public string Success { get; set; }
            public string Customer_ID { get; set; }

            public string Response { get; set; }

           
        
    }
    public class JointCust
    {
        public ClsJointAccountFinacle objroot { get; set;}
    }

        
}
