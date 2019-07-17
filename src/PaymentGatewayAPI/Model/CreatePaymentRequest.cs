namespace PaymentGatewayAPI.Model
{
    public class CreatePaymentRequest
    {
        /// <summary>
        /// Card number the transaction needs to carried out on
        /// </summary>
        public string CardNumber { get; set; }
        
        /// <summary>
        /// The expiry month of the credit card
        /// </summary>
        public int ExpiryMonth { get; set; }
        
        /// <summary>
        /// The expiry month of the credit card
        /// </summary>
        public int ExpiryYear { get; set; }
        
        /// <summary>
        /// Amount of the transaction
        /// </summary>
        public int Amount { get; set; }
        
        /// <summary>
        /// The currency of the transaction
        /// </summary>
        public string Currency { get; set; }
        
        /// <summary>
        /// Cvv number of the credit card
        /// </summary>
        public int Cvv { get; set; }
    }
}