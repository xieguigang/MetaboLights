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
   */
   function requestRepository(): object;
}
