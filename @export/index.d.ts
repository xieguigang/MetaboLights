// export R# source type define for javascript/typescript language
//
// package_source=MetaboLights

declare namespace MetaboLights {
   module _ {
      /**
      */
      function onLoad(): object;
   }
   /**
     * @param outputdir default value Is ``./``.
   */
   function extract_spectra(maf: any, rawfiles: any, outputdir?: any): object;
   /**
     * @param outputdir default value Is ``./``.
     * @param highlights default value Is ``null``.
   */
   function pathmaps(outputdir?: any, highlights?: any): object;
   /**
   */
   function requestRepository(): object;
}
