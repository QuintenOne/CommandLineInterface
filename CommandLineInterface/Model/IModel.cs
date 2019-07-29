using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace CommandLineInterface.Model
{
	public interface IModel {
		string TableName { get; set; }
		Guid Id { get; set; }
		string Name { get; set; }
	}
}
