using Special_Needs_Analysis_Calculator.Data.IModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Special_Needs_Analysis_Calculator.Data.Models;

public class ContactInfoModel
{
    private int PrimaryPhoneNumber { get; set; }
    private int SecondaryPhoneNumber { get; set; }
    private string Email { get; set; }
    private string Address { get; set; }

    public ContactInfoModel(int PrimaryPhoneNumber, int SecondaryPhoneNumber, string Email, string Adress)
    {
        this.PrimaryPhoneNumber = PrimaryPhoneNumber;
        this.SecondaryPhoneNumber = SecondaryPhoneNumber;
        this.Email = Email;
        this.Address = Adress;
    }
}


