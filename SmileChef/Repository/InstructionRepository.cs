using ChefApp.Models;
using ChefApp.Models.DbModels;

namespace SmileChef.Repository
{
    public class InstructionRepository : GenericRepository<Instruction>
    {
        public InstructionRepository(ChefAppContext context) : base(context)
        {
        }
    }
}
