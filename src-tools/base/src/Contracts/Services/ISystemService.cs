using Example.Project.Services;
using Simple.Services;
using System.Collections.Generic;
using Simple.Patterns;

namespace Example.Project.Services
{
    public partial interface ISystemService : IService, IService<ISystemService>
    {
        IList<TaskRunner.Result> Check();
    }
}