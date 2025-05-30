using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingCompany.UI.ViewModels.MaterialsViewModels;

namespace BuildingCompany.UI.Pages.MaterialPages;

public partial class CreateMaterialPage : ContentPage
{
    public CreateMaterialPage(CreateMaterialViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}