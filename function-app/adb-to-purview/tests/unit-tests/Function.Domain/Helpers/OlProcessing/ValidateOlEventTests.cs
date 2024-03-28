using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Function.Domain.Helpers.Parser;
using Function.Domain.Models.OL;

namespace UnitTests.Function.Domain.Helpers.OlProcessing
{
    public class ValidateOlEventTests
    {
        private readonly Mock<ILoggerFactory> loggerFactoryMock;
        private readonly IValidateOlEvent _validator;

        public ValidateOlEventTests()
        {
            this.loggerFactoryMock = new Mock<ILoggerFactory>();
            this._validator = new ValidateOlEvent(loggerFactoryMock.Object);
        }

        [Fact]
        public void Validate_SynapseOlMessageWithMultiInputs_ReturnsTrue()
        {
            // Arrange
            var olPayload = "{\"eventTime\":\"2023-11-30T22:43:26.834Z\",\"producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"schemaURL\":\"https://openlineage.io/spec/2-0-2/OpenLineage.json#/$defs/RunEvent\",\"eventType\":\"COMPLETE\",\"run\":{\"runId\":\"f1e77e15-4fa3-4523-8f70-9f8ad07aa53e\"},\"job\":{\"namespace\":\"synw-udf-dlz-dv-eu2-01,azuresynapsespark\",\"name\":\"n_b_ccw_ref_condition_raw_to_conformed_lratest_1701383875.adaptive_spark_plan\",\"facets\":{}},\"inputs\":[{\"namespace\":\"abfss://conformednpii@studfmodelnpiidveu204.dfs.core.windows.net\",\"name\":\"/reference/ref_cndtn_diag\",\"facets\":{\"dataSource\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/DatasourceDatasetFacet.json#/$defs/DatasourceDatasetFacet\",\"name\":\"abfss://conformednpii@studfmodelnpiidveu204.dfs.core.windows.net\",\"uri\":\"abfss://conformednpii@studfmodelnpiidveu204.dfs.core.windows.net\"},\"schema\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/SchemaDatasetFacet.json#/$defs/SchemaDatasetFacet\",\"fields\":[{\"name\":\"cndtn_diag_key\",\"type\":\"string\"},{\"name\":\"diag_cd\",\"type\":\"string\"},{\"name\":\"diag_cdst_typ_cd\",\"type\":\"string\"},{\"name\":\"diag_key\",\"type\":\"string\"},{\"name\":\"cndtn_nm\",\"type\":\"string\"},{\"name\":\"cndtn_key\",\"type\":\"string\"},{\"name\":\"udf_meta_obj_key\",\"type\":\"string\"},{\"name\":\"udf_src_del_flg\",\"type\":\"string\"},{\"name\":\"udf_created_dt\",\"type\":\"timestamp\"},{\"name\":\"udf_created_by\",\"type\":\"string\"},{\"name\":\"udf_updated_dt\",\"type\":\"timestamp\"},{\"name\":\"udf_updated_by\",\"type\":\"string\"},{\"name\":\"udf_row_hash\",\"type\":\"string\"},{\"name\":\"udf_src_meta_obj_key\",\"type\":\"string\"}]},\"symlinks\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/SymlinksDatasetFacet.json#/$defs/SymlinksDatasetFacet\",\"identifiers\":[{\"namespace\":\"/reference\",\"name\":\"reference.ref_cndtn_diag\",\"type\":\"TABLE\"}]}},\"inputFacets\":{}},{\"namespace\":\"abfss://rawhistlatestnpii@studfrawnpiidveu202.dfs.core.windows.net\",\"name\":\"/raw/chronic_condition_1997_2021/ccw_chronic_condition_algorithms_1997_2021_current\",\"facets\":{\"dataSource\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/DatasourceDatasetFacet.json#/$defs/DatasourceDatasetFacet\",\"name\":\"abfss://rawhistlatestnpii@studfrawnpiidveu202.dfs.core.windows.net\",\"uri\":\"abfss://rawhistlatestnpii@studfrawnpiidveu202.dfs.core.windows.net\"},\"schema\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/SchemaDatasetFacet.json#/$defs/SchemaDatasetFacet\",\"fields\":[{\"name\":\"algorithms\",\"type\":\"string\"},{\"name\":\"reference_period\",\"type\":\"string\"},{\"name\":\"valid_ICD09_codes\",\"type\":\"string\"},{\"name\":\"valid_ICD10_codes\",\"type\":\"string\"},{\"name\":\"number_or_type_of_claims\",\"type\":\"string\"},{\"name\":\"udf_file_name\",\"type\":\"string\"},{\"name\":\"udf_year\",\"type\":\"string\"},{\"name\":\"udf_mon\",\"type\":\"string\"},{\"name\":\"udf_day\",\"type\":\"string\"},{\"name\":\"udf_upsert_timestamp\",\"type\":\"timestamp\"},{\"name\":\"udf_ingested_date\",\"type\":\"date\"}]},\"symlinks\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/SymlinksDatasetFacet.json#/$defs/SymlinksDatasetFacet\",\"identifiers\":[{\"namespace\":\"/raw/chronic_condition_1997_2021\",\"name\":\"raw.ccw_chronic_condition_algorithms_1997_2021_current\",\"type\":\"TABLE\"}]}},\"inputFacets\":{}},{\"namespace\":\"abfss://rawhistlatestnpii@studfrawnpiidveu202.dfs.core.windows.net\",\"name\":\"/raw/chronic_condition_1997_2021/ccw_chronic_condition_algorithms_1997_2021_current\",\"facets\":{\"dataSource\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/DatasourceDatasetFacet.json#/$defs/DatasourceDatasetFacet\",\"name\":\"abfss://rawhistlatestnpii@studfrawnpiidveu202.dfs.core.windows.net\",\"uri\":\"abfss://rawhistlatestnpii@studfrawnpiidveu202.dfs.core.windows.net\"},\"schema\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/SchemaDatasetFacet.json#/$defs/SchemaDatasetFacet\",\"fields\":[{\"name\":\"algorithms\",\"type\":\"string\"},{\"name\":\"reference_period\",\"type\":\"string\"},{\"name\":\"valid_ICD09_codes\",\"type\":\"string\"},{\"name\":\"valid_ICD10_codes\",\"type\":\"string\"},{\"name\":\"number_or_type_of_claims\",\"type\":\"string\"},{\"name\":\"udf_file_name\",\"type\":\"string\"},{\"name\":\"udf_year\",\"type\":\"string\"},{\"name\":\"udf_mon\",\"type\":\"string\"},{\"name\":\"udf_day\",\"type\":\"string\"},{\"name\":\"udf_upsert_timestamp\",\"type\":\"timestamp\"},{\"name\":\"udf_ingested_date\",\"type\":\"date\"}]},\"symlinks\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/SymlinksDatasetFacet.json#/$defs/SymlinksDatasetFacet\",\"identifiers\":[{\"namespace\":\"/raw/chronic_condition_1997_2021\",\"name\":\"raw.ccw_chronic_condition_algorithms_1997_2021_current\",\"type\":\"TABLE\"}]}},\"inputFacets\":{}},{\"namespace\":\"abfss://rawhistlatestnpii@studfrawnpiidveu202.dfs.core.windows.net\",\"name\":\"/raw/chronic_condition/ccw_chronic_condition_algorithms_current\",\"facets\":{\"dataSource\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/DatasourceDatasetFacet.json#/$defs/DatasourceDatasetFacet\",\"name\":\"abfss://rawhistlatestnpii@studfrawnpiidveu202.dfs.core.windows.net\",\"uri\":\"abfss://rawhistlatestnpii@studfrawnpiidveu202.dfs.core.windows.net\"},\"schema\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/SchemaDatasetFacet.json#/$defs/SchemaDatasetFacet\",\"fields\":[{\"name\":\"algorithms\",\"type\":\"string\"},{\"name\":\"reference_period\",\"type\":\"string\"},{\"name\":\"valid_ICD10_codes\",\"type\":\"string\"},{\"name\":\"number_or_type_of_claims\",\"type\":\"string\"},{\"name\":\"udf_file_name\",\"type\":\"string\"},{\"name\":\"udf_year\",\"type\":\"string\"},{\"name\":\"udf_mon\",\"type\":\"string\"},{\"name\":\"udf_day\",\"type\":\"string\"},{\"name\":\"udf_upsert_timestamp\",\"type\":\"timestamp\"},{\"name\":\"udf_ingested_date\",\"type\":\"date\"}]},\"symlinks\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/SymlinksDatasetFacet.json#/$defs/SymlinksDatasetFacet\",\"identifiers\":[{\"namespace\":\"/raw/chronic_condition\",\"name\":\"raw.ccw_chronic_condition_algorithms_current\",\"type\":\"TABLE\"}]}},\"inputFacets\":{}},{\"namespace\":\"abfss://rawhistlatestnpii@studfrawnpiidveu202.dfs.core.windows.net\",\"name\":\"/raw/chronic_condition/ccw_chronic_condition_algorithms_current\",\"facets\":{\"dataSource\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/DatasourceDatasetFacet.json#/$defs/DatasourceDatasetFacet\",\"name\":\"abfss://rawhistlatestnpii@studfrawnpiidveu202.dfs.core.windows.net\",\"uri\":\"abfss://rawhistlatestnpii@studfrawnpiidveu202.dfs.core.windows.net\"},\"schema\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/SchemaDatasetFacet.json#/$defs/SchemaDatasetFacet\",\"fields\":[{\"name\":\"algorithms\",\"type\":\"string\"},{\"name\":\"reference_period\",\"type\":\"string\"},{\"name\":\"valid_ICD10_codes\",\"type\":\"string\"},{\"name\":\"number_or_type_of_claims\",\"type\":\"string\"},{\"name\":\"udf_file_name\",\"type\":\"string\"},{\"name\":\"udf_year\",\"type\":\"string\"},{\"name\":\"udf_mon\",\"type\":\"string\"},{\"name\":\"udf_day\",\"type\":\"string\"},{\"name\":\"udf_upsert_timestamp\",\"type\":\"timestamp\"},{\"name\":\"udf_ingested_date\",\"type\":\"date\"}]},\"symlinks\":{\"_producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"_schemaURL\":\"https://openlineage.io/spec/facets/1-0-0/SymlinksDatasetFacet.json#/$defs/SymlinksDatasetFacet\",\"identifiers\":[{\"namespace\":\"/raw/chronic_condition\",\"name\":\"raw.ccw_chronic_condition_algorithms_current\",\"type\":\"TABLE\"}]}},\"inputFacets\":{}}],\"outputs\":[]}";
            var olEvent = JsonConvert.DeserializeObject<Event>(olPayload);

            // Act
            Xunit.Assert.NotNull(olEvent);
            var actual = _validator.Validate(olEvent);

            // Assert
            Xunit.Assert.True(actual);
        }

        [Fact]
        public void Validate_SynapseOlMessageWithNoIO_ReturnsFalse()
        {
            // Arrange
            var olPayload = "{\"eventTime\":\"2023-11-30T22:43:26.834Z\",\"producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"schemaURL\":\"https://openlineage.io/spec/2-0-2/OpenLineage.json#/$defs/RunEvent\",\"eventType\":\"COMPLETE\",\"run\":{\"runId\":\"f1e77e15-4fa3-4523-8f70-9f8ad07aa53e\"},\"job\":{\"namespace\":\"synw-udf-dlz-dv-eu2-01,azuresynapsespark\",\"name\":\"n_b_ccw_ref_condition_raw_to_conformed_lratest_1701383875.adaptive_spark_plan\",\"facets\":{}},\"inputs\":[],\"outputs\":[]}";
            var olEvent = JsonConvert.DeserializeObject<Event>(olPayload);

            // Act
            Xunit.Assert.NotNull(olEvent);
            var actual = _validator.Validate(olEvent);

            // Assert
            Xunit.Assert.False(actual);
        }

        [Fact]
        public void Validate_SynapseOlMessageWithNoIO_OutputExists_ReturnsTrue()
        {
            // Arrange
            var olPayload = "{\"eventTime\":\"2023-12-05T18:07:00.6Z\",\"producer\":\"https://github.com/OpenLineage/OpenLineage/tree/1.4.1/integration/spark\",\"schemaURL\":\"https://openlineage.io/spec/2-0-2/OpenLineage.json#/$defs/RunEvent\",\"eventType\":\"COMPLETE\",\"run\":{\"runId\":\"f51d312d-adf0-4187-ac90-23d563a6d21e\"},\"job\":{\"namespace\":\"synw-udf-dlz-dv-eu2-01,azuresynapsespark\",\"name\":\"n_b_ccw_ref_condition_raw_to_conformed_lratest_1701799232.execute_merge_into_command.reference_ref_cndtn\",\"facets\":{}},\"inputs\":[],\"outputs\":[{\"namespace\":\"abfss://conformednpii@studfmodelnpiidveu204.dfs.core.windows.net\",\"name\":\"/reference/ref_cndtn\",\"outputFacets\":{}}]}";
            var olEvent = JsonConvert.DeserializeObject<Event>(olPayload);

            // Act
            Xunit.Assert.NotNull(olEvent);
            var actual = _validator.Validate(olEvent);

            // Assert
            Xunit.Assert.True(actual);
        }
    }
}