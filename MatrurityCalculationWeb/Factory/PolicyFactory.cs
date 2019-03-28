using StructureMap;
using MaturityCalculation.Business.MaturityCalculator;


namespace MaturityCalculation.Business.PolicyManagement.Implementation
{
    public class PolicyFactory : IPolicyFactory
    {
        private readonly IContainer container;

        public PolicyFactory(IContainer _container)
        {
            container = _container;
        }

        public IMaturityCalculator Create(string policyType)
        {
            return container.GetInstance<IMaturityCalculator>(policyType);
        }        
    }
}
