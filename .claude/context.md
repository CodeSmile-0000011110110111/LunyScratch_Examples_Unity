# Claude Session Context

## Project
- Unity project: LunyScratch Examples (actually: LunyScript Examples)
- Package: de.codesmile.lunyscript (submodule)
- Branch: main
- Cross-Engine: Luny/LunyScript execute in Unity, Godot, other C# engines
- Package location: Packages/de.codesmile.lunyscript
  - 'Luny/' contains general-purpose (not just LunyScript) engine-agnostic types
  - 'Luny.Unity/' contains Unity-specific implementations/bindings
  - 'LunyScript/' contains engine-agnostic scripting types, utilizing Luny library
  - 'LunyScript.Unity/' contains Unity-specific bindings for LunyScript
  - analogous Luny.Godot and LunyScript.Godot exist in the Godot addon repository
- Code restricted to C# 9 for compatibility with Unity 6
- LunyScriptRunner runs IRunnable instances (sequences, statemachines, behavior trees) which then execute (or query) IBlock instances

## Implementation Notes
- for logging, prefer LunyLogger methods (it redirects to engine-native logging, or in absence to Console.WriteLine)

## Preferences
- Concise, direct responses
- Ask if a new preference from prompts should be added to context.md
- End all responses with: `#Credits spent in chat: ~$X.XX (+$X.XX), Context: XX% budget` (credits = high estimate, accumulative for entire chat; in brackets: by how much value changed since last prompt)
- **Step-by-Step Mode**: Always operate in a strict planning-first mode.
- **Explicit Confirmation**: Do not modify any files or start implementation until the user has explicitly confirmed the plan.
- **No "Running Ahead"**: Wait for confirmation after each major phase (Planning -> Implementation -> Verification).

## Code Style
- don't write if or else statements on a single line
- don't add braces around single-line statements (if, else, for, ..) except for do/while and using
- properties and methods whenever possible should be written on a single line using "expression" syntax - use a ternary if needed but avoid multiple (nested) ternaries
- interfaces for only one type should be added to the same file as the type implementing it. Place the interface above the type.

## Notes
- Test scene: in package, in engine-specific folders
