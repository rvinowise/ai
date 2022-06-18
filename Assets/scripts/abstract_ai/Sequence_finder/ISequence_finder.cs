

using System.Collections.Generic;

namespace rvinowise.ai.general {
public interface ISequence_finder<TFigure>
where TFigure: class?, IFigure
{


  
    void enrich_storage_with_sequences();



}
}