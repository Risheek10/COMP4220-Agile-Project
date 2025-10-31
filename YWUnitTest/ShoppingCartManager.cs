using System;


namespace ywBookStoreLIB
{
    // Simple singleton-ish access to the current shopping cart
    public static class ShoppingCartManager
    {
        public static BookOrder CurrentOrder { get; } = new BookOrder();
    }
}