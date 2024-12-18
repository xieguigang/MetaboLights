﻿// export R# source type define for javascript/typescript language
//
// package_source=MetaboLights

declare namespace MetaboLights {
   module _ {
      /**
      */
      function onLoad(): object;
   }
   /**
   */
   function combine_sampleinfo(x: any, sampleinfo: any): object;
   /**
     * @param outputdir default value Is ``./``.
   */
   function extract_spectra(maf: any, rawfiles: any, outputdir?: any): object;
   /**
     * @param outputdir default value Is ``./``.
     * @param highlights default value Is ``null``.
     * @param default.fill_color default value Is ``lightgray``.
   */
   function pathmaps(outputdir?: any, highlights?: any, default.fill_color?: any): object;
   /**
   */
   function pathway_metabolites(cas_id: any): object;
   /**
   */
   function readJSON(name: any, what: any): object;
   /**
   */
   function requestRepository(): object;
}
