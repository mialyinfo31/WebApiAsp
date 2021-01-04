using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIwebCore.Model
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }


        //ETAPE I sécurité :
        //Ajouter des champs secrets pour la sécurité
        public String Secret { get; set; }

    }
}
