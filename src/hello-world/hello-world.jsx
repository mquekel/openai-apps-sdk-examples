import React from "react";
import { Sparkles } from "lucide-react";

/**
 * Hello World Component
 * 
 * A simple example component demonstrating the basic structure and styling
 * patterns used in this repository. This serves as a starting point for
 * learning how to build custom apps with the Apps SDK.
 */
function App() {
  return (
    <div className="antialiased w-full text-black p-6 border border-black/10 rounded-3xl bg-gradient-to-br from-white to-blue-50 shadow-lg">
      <div className="max-w-md mx-auto">
        {/* Header */}
        <div className="flex items-center gap-3 mb-6">
          <div className="p-3 bg-blue-500 rounded-xl shadow-md">
            <Sparkles className="h-8 w-8 text-white" strokeWidth={2} />
          </div>
          <div>
            <h1 className="text-2xl font-bold text-black">Hello World</h1>
            <p className="text-sm text-black/60">Your first component</p>
          </div>
        </div>

        {/* Content */}
        <div className="bg-white rounded-2xl p-6 shadow-sm border border-black/5">
          <h2 className="text-lg font-semibold mb-3 text-black">
            Welcome to the Apps SDK!
          </h2>
          <p className="text-sm text-black/70 leading-relaxed mb-4">
            This is a simple example component to help you get started building
            your own apps. It demonstrates:
          </p>
          
          <ul className="space-y-2 mb-6">
            <li className="flex items-start gap-2">
              <span className="text-blue-500 font-bold">•</span>
              <span className="text-sm text-black/70">
                Basic React component structure
              </span>
            </li>
            <li className="flex items-start gap-2">
              <span className="text-blue-500 font-bold">•</span>
              <span className="text-sm text-black/70">
                Tailwind CSS styling with this repository's patterns
              </span>
            </li>
            <li className="flex items-start gap-2">
              <span className="text-blue-500 font-bold">•</span>
              <span className="text-sm text-black/70">
                Using Lucide React icons
              </span>
            </li>
            <li className="flex items-start gap-2">
              <span className="text-blue-500 font-bold">•</span>
              <span className="text-sm text-black/70">
                How to export and mount the component
              </span>
            </li>
          </ul>

          <div className="p-4 bg-blue-50 rounded-xl border border-blue-100">
            <p className="text-xs font-medium text-blue-900 mb-1">
              💡 Next Steps
            </p>
            <p className="text-xs text-blue-800">
              Check out the other components in the <code className="bg-blue-100 px-1 py-0.5 rounded">src/</code> directory
              to see more complex examples, then create your own!
            </p>
          </div>
        </div>

        {/* Footer */}
        <div className="mt-6 text-center">
          <button
            type="button"
            className="px-6 py-2.5 bg-blue-500 hover:bg-blue-600 text-white font-medium rounded-full shadow-md hover:shadow-lg transition-all duration-200 active:scale-95"
          >
            Get Started
          </button>
        </div>
      </div>
    </div>
  );
}

export default App;
