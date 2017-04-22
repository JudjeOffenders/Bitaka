using System.ComponentModel.DataAnnotations;

namespace ALS.Data
{

    public enum Cities
    {
        Sofia,
        Plovdiv,
        Varna,
        Burgas,
        Ruse,
        [Display(Name = "Stara Zagora")]
        StaraZagora,
        Pleven,
        Dobrich,
        Sliven,
        Shumen,
        Pernik,
        Haskovo,
        Yambol,
        Pazardzhik,
        Blagoevgrad,
        [Display(Name = "Veliko Tarnovo")]
        VelikoTarnovo,
        Vratza,
        Gabrovo,
        Asenovgrad,
        Vidin,
        Kazanlak
    }
}