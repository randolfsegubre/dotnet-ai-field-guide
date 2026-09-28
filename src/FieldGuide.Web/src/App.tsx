import { useState } from "react";
import { askChatDemo, type AskResponse } from "./api/chatDemo";
import "./App.css";

/**
 * Topic 1's "Try it" panel: send a question, see the model's answer, and see whether it
 * actually called the get_current_date_time tool instead of guessing. This is deliberately
 * plain, no component library, so the React + fetch + API round trip is easy to trace end
 * to end when you come back to re-read this later.
 */
function App() {
  const [message, setMessage] = useState("What time is it right now in Asia/Manila?");
  const [result, setResult] = useState<AskResponse | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  async function handleAsk() {
    setIsLoading(true);
    setError(null);
    setResult(null);
    try {
      const response = await askChatDemo(message);
      setResult(response);
    } catch (err) {
      setError(err instanceof Error ? err.message : String(err));
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <main className="field-guide">
      <h1>.NET AI Field Guide</h1>
      <p>
        Topic 1: <code>IChatClient</code> basics and tool calling. This calls the real
        FieldGuide.Api, which calls a local Ollama model, and saves the exchange to SQL Server.
      </p>

      <textarea
        value={message}
        onChange={(e) => setMessage(e.target.value)}
        rows={2}
        style={{ width: "100%", maxWidth: 640 }}
      />
      <div>
        <button onClick={handleAsk} disabled={isLoading || !message.trim()}>
          {isLoading ? "Asking..." : "Ask"}
        </button>
      </div>

      {error && <p style={{ color: "crimson" }}>Error: {error}</p>}

      {result && (
        <section>
          <h2>Answer</h2>
          <p>{result.answer}</p>
          <h3>Tool calls invoked</h3>
          <p>{result.toolCallsInvoked.length > 0 ? result.toolCallsInvoked.join(", ") : "(none)"}</p>
          <p><small>Saved as transcript #{result.transcriptId} in SQL Server.</small></p>
        </section>
      )}
    </main>
  );
}

export default App;
