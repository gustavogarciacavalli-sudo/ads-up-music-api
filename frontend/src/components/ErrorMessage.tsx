interface Props {
  message: string;
}

export default function ErrorMessage({ message }: Props) {
  return (
    <div className="error-message">
      <span className="error-icon">⚠️</span>
      <p>{message}</p>
    </div>
  );
}
