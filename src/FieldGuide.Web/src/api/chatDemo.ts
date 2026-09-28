// Thin wrapper around FieldGuide.Api's ChatDemoController. One function per endpoint,
// no framework magic, so it's easy to read even months from now.

// Matches src/FieldGuide.Api/Properties/launchSettings.json's "https" profile.
// If you regenerate launchSettings.json, dotnet may pick a different random port; update here to match.
const API_BASE_URL = "https://localhost:7185";

export interface AskResponse {
  answer: string;
  toolCallsInvoked: string[];
  transcriptId: number;
}

export interface ChatTranscript {
  id: number;
  topicSlug: string;
  modelId: string;
  userMessage: string;
  answer: string;
  toolCallsInvoked: string;
  createdAtUtc: string;
}

export async function askChatDemo(message: string): Promise<AskResponse> {
  const response = await fetch(`${API_BASE_URL}/api/chat/demo`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ message }),
  });

  if (!response.ok) {
    throw new Error(`Chat demo request failed: ${response.status} ${await response.text()}`);
  }

  return response.json();
}

export async function getChatHistory(): Promise<ChatTranscript[]> {
  const response = await fetch(`${API_BASE_URL}/api/chat/history`);

  if (!response.ok) {
    throw new Error(`History request failed: ${response.status}`);
  }

  return response.json();
}
