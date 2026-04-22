# Pattern 6: Evolutionary & Emergent Systems

These turn stateless subagents into something that feels alive. All build on the filesystem blackboard.

| Variant                        | How It Works                                                                                                                      | Best For                                      |
| ------------------------------ | --------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------- |
| **Evolutionary Optimization**  | Spawn 8-20 agents as a "generation"; evaluator scores; "breeder" creates next-gen instructions from winners; run 5-10 generations | Optimizing algorithms, UI designs, strategies |
| **Stakeholder Simulation**     | Agents are characters (customer, competitor, regulator) acting on shared "world state" files in turns                             | Product strategy, risk analysis               |
| **Swarm Intelligence**         | Dozens of lightweight agents explore solution space, depositing "pheromone" scores; later agents bias toward high-scoring paths   | Broad coverage with minimal planning          |
| **Recursive Meta-Improvement** | "Evolver" agents analyze past logs and propose improved system prompts, new roles, or better orchestration heuristics             | System self-improvement across sessions       |
