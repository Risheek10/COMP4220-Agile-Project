using System;
using System.ComponentModel;


namespace ywBookStoreLIB
{
    public class OrderItem : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion


        private string _bookID;
        private string _bookTitle;
        private int _quantity;
        private double _unitPrice;
        private double _subTotal;


        public string BookID
        {
            get => _bookID;
            set { if (_bookID != value) { _bookID = value; Notify(nameof(BookID)); } }
        }
        public string BookTitle
        {
            get => _bookTitle;
            set { if (_bookTitle != value) { _bookTitle = value; Notify(nameof(BookTitle)); } }
        }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0) value = 0; // guard
                if (_quantity != value)
                {
                    _quantity = value;
                    UpdateSubTotal();
                    Notify(nameof(Quantity));
                }
            }
        }
        public double UnitPrice
        {
            get => _unitPrice;
            set { if (_unitPrice != value) { _unitPrice = value; UpdateSubTotal(); Notify(nameof(UnitPrice)); } }
        }
        public double SubTotal
        {
            get => _subTotal;
            private set { if (_subTotal != value) { _subTotal = value; Notify(nameof(SubTotal)); } }
        }


        private void UpdateSubTotal()
        {
            SubTotal = UnitPrice * Quantity;
        }


        public OrderItem(String isbn, String title,
        double unitPrice, int quantity)
        {
            _bookID = isbn;
            _bookTitle = title;
            _unitPrice = unitPrice;
            _quantity = quantity;
            UpdateSubTotal();
        }
        public override string ToString()
        {
            string xml = "<OrderItem ISBN='" + BookID + "'";
            xml += " Quantity='" + Quantity + "' />";
            return xml;
        }
    }
}
