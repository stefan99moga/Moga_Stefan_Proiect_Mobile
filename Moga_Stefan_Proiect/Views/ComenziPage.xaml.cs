using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moga_Stefan_Proiect.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComenziPage : ContentPage
    {
        public ComenziPage()
        {
            InitializeComponent();
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void listOrders_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}