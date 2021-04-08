using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Enums;
using Hurace.Core.Extensions;
using Hurace.Core.Logic.Models;
using Hurace.Core.Models;
using Hurace.Core.Validators;
using Hurace.Data;

namespace Hurace.Core.Logic
{
    public class SkierLogic : ISkierLogic
    {
        private readonly ISkierManager skierManager;
        private readonly SkierValidator validator;

        public SkierLogic(
            ISkierManager skierManager,
            SkierValidator validator)
        {
            this.skierManager = skierManager ?? throw new ArgumentNullException(nameof(skierManager));
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<IEnumerable<Skier>> GetAllAsync(Gender gender, bool isActive = true)
        {
            var skiers = await this.skierManager.GetAllAsync(gender, isActive);
            return skiers.OrderBy(s => s.FullName()).ToList();
        }

        public async Task<IEnumerable<Skier>> GetAllAsync(bool? isActive = null)
        {
            var skiers = await this.skierManager.GetAllAsync(isActive);
            return skiers.OrderBy(s => s.FullName()).ToList();
        }

        public async Task<Skier> GetByIdAsync(int id)
        {
            return await this.skierManager.GetByIdAsync(id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            if (id == 0)
            {
                return true;
            }

            return await this.skierManager.RemoveAsync(id);
        }

        public async Task<SaveResult> SaveAsync(Skier skier)
        {
            var result = this.validator.Validate(skier);
            if (!result.IsValid)
            {
                return new SaveResult.ValidationError(result.Errors);
            }

            if (skier.Id == 0)
            {
                await this.skierManager.CreateAsync(skier);
            }
            else
            {
                bool updateSuccess = await this.skierManager.UpdateAsync(skier);
                if (!updateSuccess)
                {
                    return new SaveResult.Error(ErrorCode.UpdateError);
                }
            }

            return new SaveResult.Success(skier.Id);
        }
    }
}
