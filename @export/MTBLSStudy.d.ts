// export R# package module type define for javascript/typescript language
//
//    imports "MTBLSStudy" from "MetaboLights";
//
// ref=MetaboLights.MTBLSStudy@MetaboLights, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * MTBLS study project data reader
 * 
*/
declare namespace MTBLSStudy {
   module read {
      /**
        * @param env default value Is ``null``.
      */
      function study_source(file: any, env?: object): object;
   }
   /**
    * 
    * > the sample group information is generates via the combination of 
    * >  **`group`** and **`property`** data, 
    * >  example as, there is data field named ``Factor Value[Cohort]`` in maf 
    * >  table file, then the group parameter value should be ``factor`` and 
    * >  the property parameter value should be ``Cohort``.
    * 
     * @param metadata -
     * @param group the group source, value could be characteristics or factor
     * @param property the group property data, value maybe various based on the study details
   */
   function sampleinfo(metadata: object, group: string, property: string): object;
}
