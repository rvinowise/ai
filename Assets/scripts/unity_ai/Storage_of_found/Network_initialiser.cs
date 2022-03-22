using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.table;
using UnityEngine;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity {
public class Network_initialiser: 
    MonoBehaviour,
    INetwork_initialiser
{
    private ai.simple.Network_initialiser simple_initialiser;
    
    public IFigure_storage figure_storage;
    public IFigure_provider figure_provider;
    [SerializeField] private Mode_selector mode_selector; 
    
    void Awake() {
        simple_initialiser = new ai.simple.Network_initialiser(
            figure_storage,
            create_base_signal
        );
    }
    void Start() {
        create_base_signals();
    }

    public void create_base_signals() {
        simple_initialiser.create_base_signals();
    }

    private IFigure create_base_signal(string id) {
        Figure signal = figure_provider.create_new_figure(id) as unity.Figure;
        signal.name = $"signal {signal.id}";
        signal.header.mode_selector = mode_selector;
        return signal;
    }
    
    
}
}