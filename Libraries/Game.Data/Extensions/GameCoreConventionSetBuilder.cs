using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Data.Extensions
{
    public class GameCoreConventionSetBuilder : CoreConventionSetBuilder
    {
        public GameCoreConventionSetBuilder(CoreConventionSetBuilderDependencies dependencies) : base(dependencies) { }
        public override ConventionSet CreateConventionSet()
        {
            var conventionSet = base.CreateConventionSet();
            //默认字符串长度，而不是nvarchar(max).
            //为什么要insert(0,obj),则是因为这个默认规则要最优先处理，如果后续有规则的话就直接覆盖了。
            //propertyBuilder.HasMaxLength(32, ConfigurationSource.Convention);
            //理论上我指定了规则的级别为.Convention，应该和顺序就没有关系了。but，还没有完成测试，所以我也不知道
            conventionSet.PropertyAddedConventions.Insert(0, new StringDefaultLengthConvention());
            //decimal设置精度
            conventionSet.PropertyAddedConventions.Add(new DecimalPrecisionAttributeConvention());
            return conventionSet;
        }
    }
}
