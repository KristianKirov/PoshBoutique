using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Models
{
    public class SizeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int OrderIndex { get; set; }

        private List<ColorModel> colors;

        public IEnumerable<ColorModel> Colors
        {
            get
            {
                return colors;
            }
        }

        public void AddColor(Color color, int quantity)
        {
            if (this.colors == null)
            {
                this.colors = new List<ColorModel>();
            }

            this.Quantity += quantity;
            this.colors.Add(new ColorModel()
                {
                    Id = color.Id,
                    Title = color.Title,
                    Quantity = quantity
                });
        }
    }
}
