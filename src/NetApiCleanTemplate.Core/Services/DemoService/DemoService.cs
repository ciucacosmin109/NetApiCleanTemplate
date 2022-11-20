using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.Core.Entities.DemoEntity;
using NetApiCleanTemplate.Core.Entities.DemoEntity.Guards;
using NetApiCleanTemplate.Core.General.Guards;
using NetApiCleanTemplate.Core.Services.DemoService.Input;
using NetApiCleanTemplate.Core.Services.DemoService.Output;
using NetApiCleanTemplate.SharedKernel.Guards;
using NetApiCleanTemplate.SharedKernel.Interfaces;
using NetApiCleanTemplate.SharedKernel.Interfaces.Uow;

namespace NetApiCleanTemplate.Core.Services.DemoService;
public class DemoService : IDemoService
{
    private readonly IRepository<DemoEntity> demoRepo;
    private readonly IUnitOfWorkManager uowManager;

    public DemoService(
        IRepository<DemoEntity> demoRepo,
        IUnitOfWorkManager uowManager
    ) {
        this.demoRepo = demoRepo;
        this.uowManager = uowManager;
    }

    public async Task ChangeParent(ChangeParentForDemoDto dto)
    {
        using var uow = uowManager.Begin();

        var demo = await demoRepo.Get(dto.Id);
        Guard.Against.NonExistentEntity(demo, dto.Id);

        demo!.DemoParentId = dto.DemoParentId;
        await demoRepo.Update(demo);

        throw new Exception();

        uow.Complete();
    }

    public async Task<int> Create(CreateDemoDto dto)
    {
        Guard.Against.InvalidDemoString(dto.DemoString ?? "");

        var demo = new DemoEntity {
            DemoString = dto.DemoString ?? "",
            DemoParentId = dto.DemoParentId,
        };
        var dbDemo = await demoRepo.Insert(demo);
        return dbDemo.Id;
    }

    public async Task Delete(int id)
    {
        await demoRepo.Delete(id);
    }

    public async Task<DemoDto> Get(int id)
    {
        var demo = await demoRepo.Get(id);
        Guard.Against.NonExistentEntity(demo, id);

        return DemoDto.ToDto(demo!);
    }

    public async Task<ICollection<DemoDto>> GetAll()
    {
        var demos = await demoRepo.GetAll();
        return DemoDto.ToDto(demos.ToList());
    }

    public async Task Update(UpdateDemoDto dto)
    {
        var demo = await demoRepo.Get(dto.Id);
        Guard.Against.NonExistentEntity(demo, dto.Id);

        demo!.DemoString = dto.DemoString ?? "";
        await demoRepo.Update(demo);
    }
}
