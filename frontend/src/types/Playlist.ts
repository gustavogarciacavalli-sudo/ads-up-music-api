export interface Playlist {
  id: number;
  name: string;
  description: string;
  mood: string;
  createdAt: string;
}

export interface PlaylistSummary {
  name: string;
  trackCount: number;
  averageBpm: number;
  totalDuration: string;
  artists: string[];
  genres: string[];
}
