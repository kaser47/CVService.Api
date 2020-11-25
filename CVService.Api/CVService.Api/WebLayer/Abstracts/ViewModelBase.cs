using CVService.Api.CommonLayer.Abstracts;

namespace CVService.Api.WebLayer.Abstracts
{
    //TODO: Tech Test - while not every api view model will have an Id and a Name,
    //the majority will and by extending this baseviewmodel, we can write DRY code
    //like in the acceptancetests where INameable is used to update the name. 
    public abstract class ViewModelBase : IHasId, INameable
    {
        public int Id { get; set; }

        public abstract string Name { get; set; }
    }
}